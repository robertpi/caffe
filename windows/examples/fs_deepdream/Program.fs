open System
open System.IO
open System.Drawing
open System.Drawing.Imaging
open System.Runtime.InteropServices
open Caffe.Clr

let numChannels = 3

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

        let inputData = PseudoMatrices.splitRollCombine inputData width height xJitter yJitter

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

        let inputData'' = PseudoMatrices.splitRollCombine inputData' width height -xJitter -yJitter

        inputBlob.SetData(inputData'')

    inputBlob.GetData()


[<EntryPoint>]
let main argv = 
    let modelFile   = argv.[0]
    let trainedFile = argv.[1]
    let meanFile    = argv.[2]

    // unpack the argument for the files to be tested
    let imgFile = argv.[3]

    let net = new Net(modelFile, Phase.Train)
    net.CopyTrainedLayersFrom(trainedFile)

    let inputBlob = net.InputBlobs |> Seq.head
    let outputBlob = net.OutputBlobs |> Seq.head

    let bitmap = Image.FromFile(imgFile) :?> Bitmap
    let bitmapResized = DotNetImaging.resizeBitmap bitmap 1000.
    let allChannels = DotNetImaging.formatBitmapAsBgrChannels bitmapResized
    let size = bitmapResized.Size
    let mean = BlobHelpers.loadMean meanFile numChannels size.Width size.Height
    ArrayHelpers.arraySubInPlace mean allChannels

    let layer = "inception_3b/5x5_reduce"
    let layerIndex = net.LayerNames |> Seq.findIndex(fun x -> x = layer)

    for zoomFactor in 4 .. -1 .. 2  do
        let zoomWidth, zoomHeight, subMatrices = PseudoMatrices.splitZoomCombine zoomFactor size.Width size.Height allChannels

        inputBlob.Reshape([|1; numChannels; zoomWidth; zoomHeight |])
        net.Reshape()

        let treadedParts =
            Array.map (fun subMatrix -> 
                makeStep  net inputBlob zoomWidth zoomHeight subMatrix layerIndex layer mean) subMatrices 

        PseudoMatrices.unzoom zoomFactor size.Width size.Height treadedParts allChannels
        let imageToSave = ArrayHelpers.arrayAdd mean allChannels
        DotNetImaging.saveImageDotNet imageToSave (numberedFileName imgFile layer "" zoomFactor) size

    inputBlob.Reshape([|1; numChannels; size.Width; size.Height |])
    net.Reshape()
    let finalLayer = makeStep  net inputBlob size.Width size.Height allChannels layerIndex layer mean
    let imageToSave = ArrayHelpers.arrayAdd mean finalLayer
    DotNetImaging.saveImageDotNet imageToSave (numberedFileName imgFile layer "" 0) size

    0 // return an integer exit code
