open System
open System.IO
open System.Drawing
open System.Drawing.Imaging
open System.Runtime.InteropServices
open Caffe.Clr

let arrayOfImage (bitmap: Bitmap) =
    let bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat)
    try
        let numbytes = bmpdata.Stride * bitmap.Height
        let bytedata: byte[] = Array.zeroCreate numbytes
        let ptr = bmpdata.Scan0;

        Marshal.Copy(ptr, bytedata, 0, numbytes)

        bytedata
    finally
        bitmap.UnlockBits(bmpdata)

let rollArrayX xLength shift (data:'t[])  =
    let out:'t[] = Array.zeroCreate data.Length
    let shift =
        if shift > 0 then
            shift
        else
            xLength + shift
    for i in 0 .. data.Length - 1 do
        let row = (i / xLength) * xLength
        out.[row + ((i + shift) % xLength) ] <- data.[i]
    out

let rollArrayY xLength yLength shift (data:'t[]) =
    let shift =
        if shift > 0 then
            shift
        else
            yLength + shift
    let out:'t[] = Array.zeroCreate data.Length
    for i in 0 .. data.Length - 1 do
        let shiftIndex = (i + (shift * xLength)) % data.Length
        out.[shiftIndex] <- data.[i]
    out

let zoom zoomFactor width height (data:'t[]) =
    let zoomedWidth = width / zoomFactor
    let zoomedHeight = height / zoomFactor
    zoomedWidth,
    zoomedHeight,
    [| for y in 0 .. zoomFactor - 1 do
            for x in 0 .. zoomFactor - 1 do
                let yNext = y + 1
                let xNext = x + 1
                yield [| for actualY in zoomedHeight * y .. (zoomedHeight * yNext) - 1 do
                            let hightOffset = width * actualY
                            yield! data.[ hightOffset + (zoomedWidth * x) .. hightOffset + (zoomedWidth * xNext) - 1 ] |] |]


// add these as unit test somewhere
//let test =
//    [| 0. .. 29. |]
//
//
//let printArrayBy x (data:'t[]) =
//    for i in 0 .. data.Length - 1 do
//        printf "%f\t" data.[i]
//        if i % x = x - 1 then printfn ""
//    printfn ""
//
//printArrayBy 5 test
//
//let test' = rollArrayX 5 4 test 
//printArrayBy 5 test'
//
//let test'' = rollArrayX 5 -4 test'
//printArrayBy 5 test''
//
//
//let testy' = rollArrayY 5 6 1 test 
//printArrayBy 5  testy'
//
//let testy'' = rollArrayY 5 6  -1 testy'
//printArrayBy 5 testy''
//
//let test =
//    [| 0. .. 99. |]
//
//let res = zoom 2 10 10 test
//
//for grid in res do
//    printArrayBy 5 grid
//
//unzoom 2 10 10 res test
//
//printArrayBy 10 test
//

let splitChanels (data: 't[])  =
    let chanelSize = data.Length / 3

    let b = data |> Seq.take chanelSize 
    let g = data |> Seq.skip chanelSize |> Seq.take chanelSize 
    let r = data |> Seq.skip (chanelSize * 2)

    b, g, r

let splitRollCombine inputData width height xShift yShift =
    let g,b,r = splitChanels inputData
    let rollArray  =
        (rollArrayX width xShift) >> 
        (rollArrayY width height yShift)
    [| yield! g |> Seq.toArray |> rollArray;
        yield! b |> Seq.toArray |> rollArray;
        yield! r |> Seq.toArray |> rollArray; |]

let splitZoomCombine zoomFactor width height inputData =
    let g,b,r = splitChanels inputData
    let zoomWidth, zoomHeight, g = zoom zoomFactor width height (g |> Seq.toArray)
    let _, _, b = zoom zoomFactor width height (b |> Seq.toArray)
    let _, _, r = zoom zoomFactor width height (r |> Seq.toArray)
    zoomWidth, zoomHeight,
    Array.map3 (fun g b r -> [| yield!  g; yield! b;  yield! r |]) g b r
    
let unzoom zoomFactor width height (data:seq<'t[]>) (originalMatrix:'t[]) =
    let channelLength = originalMatrix.Length / 3
    let zoomedWidth = width / zoomFactor
    let zoomedHeight = height / zoomFactor
    data |> Seq.iteri(fun i subMatrix ->
            let subX = i % zoomFactor
            let subY = i / zoomFactor
            let g,b,r = splitChanels subMatrix
            let doChannel offset (channel: 't[]) =
                for y in 0 .. zoomedHeight - 1 do            
                    let offsetX = zoomedWidth * subX
                    let offsetY = width * (subY * zoomedHeight)
                    for x in 0 .. zoomedWidth - 1 do
                        originalMatrix.[ offset + offsetY + (y * width) + offsetX + x  ] <- channel.[(y * zoomedWidth) + x]
            doChannel 0 (g |> Seq.toArray)
            doChannel channelLength (b |> Seq.toArray)
            doChannel (channelLength * 2) (r |> Seq.toArray))


let saveImageDotNet (data: float32[]) file (size: Size) =
    let bitmap = new Bitmap(size.Width, size.Height)

    let b, g, r = splitChanels data

    let rgb = Seq.zip3 r g b 

    rgb |> Seq.iteri (fun i (r, g, b)  ->
        let x = i % size.Width
        let y = i / size.Width
        let intMax255 x =
            max (min (int x) 255) 0
        let p = Color.FromArgb(intMax255 r, intMax255 g, intMax255 b)
        bitmap.SetPixel(x, y, p))

    bitmap.Save(file)

type Edge =
    | Width
    | Height

let loadMean meanFile numChannels width height =
    let meanBlob = Blob.FromProtoFile(meanFile)

    // not quite sure what the point of this resize is as we're
    // going to calculate an average, but the C++ sample did it!
    meanBlob.Reshape([|1; numChannels; width; height|])
    meanBlob.GetData() |> Seq.average

let arrayAddInPlace value (data: float32[]) =
    data
    |> Array.iteri (fun i x -> data.[i] <- (x - value))

let arraySubInPlace value (data: float32[]) =
    arrayAddInPlace -value data

let arrayAdd value (data: float32[]) =
    data
    |> Array.map (fun x -> x - value)

let arraySub value (data: float32[]) =
    arrayAdd -value data

let resizeBitmap (bitmap: Bitmap) longEdgeMax =

    let longEdge, edge = 
        if bitmap.Width > bitmap.Height then bitmap.Width |> float, Width
        else bitmap.Height |> float, Height
            
    let longEdgeTarget = min longEdge longEdgeMax 
    let scaleRatio = longEdgeTarget / longEdge 

    let width, height =
        match edge with
        | Width -> longEdgeTarget, scaleRatio * float bitmap.Height
        | Height -> scaleRatio * float bitmap.Width, longEdgeTarget

    let size = new Size(int width, int height)
    new Bitmap(bitmap, size)

let formatBitmapAsBgrChannels (bitmap: Bitmap) =
    let byteData = arrayOfImage bitmap
    let r =
        seq { for i in 0 .. 4 .. byteData.Length - 1 do
                yield float32 byteData.[i] }
    let g =
        seq { for i in 0 .. 4 .. byteData.Length - 1 do
                yield float32 byteData.[i + 1] }
    let b =
        seq { for i in 0 .. 4 .. byteData.Length - 1 do
                yield float32 byteData.[i + 2] }
    let rgb =
        seq { yield! r; yield! g; yield! b }

    rgb |> Seq.toArray


let numChannels = 3

let modelFile   = @"C:\code\mscaffe\models\bvlc_googlenet\deploy.prototxt"
let trainedFile = @"C:\code\mscaffe\models\bvlc_googlenet\bvlc_googlenet.caffemodel"
let meanFile    = @"C:\code\mscaffe\data\ilsvrc12\imagenet_mean.binaryproto"

let imgFile = @"C:\Users\rpickeri\Pictures\rob.jpg"

let rnd = new Random()

let numberedFileName filepath (layer: string) extraPart i =
    let directory = Path.GetDirectoryName(filepath)
    let filename = Path.GetFileNameWithoutExtension(filepath)
    let ext = Path.GetExtension(filepath)
    let layerSafe = layer.Replace("/", "-")
    Path.Combine(directory, sprintf "%s_%s_%s%i%s" filename layerSafe extraPart i ext)

let makeStep (net: Net) (inputBlob: Blob) width height (data: float32[]) layerIndex layer mean =
    inputBlob.SetData(data)

    for i in 1 .. 10 do
        let inputData = inputBlob.GetData()

        let xJitter, yJitter = rnd.Next(-32, 32), rnd.Next(-32, 32)

        let inputData = splitRollCombine inputData width height xJitter yJitter

        inputBlob.SetData(inputData)

        net.ForwardTo(layerIndex) |> ignore
        let outputBlob = net.BlobByName(layer)
        outputBlob.SetDiff(outputBlob.GetData())
        net.BackwardFrom(layerIndex)

        let inputData = inputBlob.GetData()
        let inputDiff = inputBlob.GetDiff()

        let absMean = inputDiff |> Seq.map Math.Abs |> Seq.average

        let inputData' =
            Seq.zip inputData inputDiff 
            |> Seq.map(fun (data, diff) -> data + (1.5f / absMean * diff))
            |> Seq.toArray

        let inputData'' = splitRollCombine inputData' width height -xJitter -yJitter

        inputBlob.SetData(inputData'')

    inputBlob.GetData()


[<EntryPoint>]
let main argv = 

    let net = new Net(modelFile, Phase.Train)
    net.CopyTrainedLayersFrom(trainedFile)

    let inputBlob = net.InputBlobs |> Seq.head
    let outputBlob = net.OutputBlobs |> Seq.head

    let bitmap = Image.FromFile(imgFile) :?> Bitmap
    let bitmapResized = resizeBitmap bitmap 1000.
    let allChannels = formatBitmapAsBgrChannels bitmapResized
    let size = bitmapResized.Size
    let mean = loadMean meanFile numChannels size.Width size.Height
    arraySubInPlace mean allChannels

    let layer = "inception_3b/5x5_reduce"
    let layerIndex = net.LayerNames |> Seq.findIndex(fun x -> x = layer)

    for zoomFactor in 4 .. -1 .. 2  do
        let zoomWidth, zoomHeight, subMatrices = splitZoomCombine zoomFactor size.Width size.Height allChannels

        inputBlob.Reshape([|1; numChannels; zoomWidth; zoomHeight |])
        net.Reshape()

        let treadedParts =
            Array.map (fun subMatrix -> 
                makeStep  net inputBlob zoomWidth zoomHeight subMatrix layerIndex layer mean) subMatrices 

        unzoom zoomFactor size.Width size.Height treadedParts allChannels
        let imageToSave = arrayAdd mean allChannels
        saveImageDotNet imageToSave (numberedFileName imgFile layer "" zoomFactor) size

    inputBlob.Reshape([|1; numChannels; size.Width; size.Height |])
    net.Reshape()
    let finalLayer = makeStep  net inputBlob size.Width size.Height allChannels layerIndex layer mean
    let imageToSave = arrayAdd mean finalLayer
    saveImageDotNet imageToSave (numberedFileName imgFile layer "" 0) size

    0 // return an integer exit code
