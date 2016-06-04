open System
open System.IO
open System.Diagnostics
open System.Drawing
open System.Drawing.Imaging
open System.Runtime.InteropServices
open Caffe.Clr

let numChannels = 3

let rnd = new Random()

let time name func =
    let clock = Stopwatch.StartNew()
    let result = func()
    printfn "%s time taken: %O" name clock.Elapsed
    result

let numberedFileName filepath (layer: string) extraPart i =
    let directory = Path.GetDirectoryName(filepath)
    let filename = Path.GetFileNameWithoutExtension(filepath)
    let ext = Path.GetExtension(filepath)
    let layerSafe = layer.Replace("/", "-")
    Path.Combine(directory, sprintf "%s_%s_%s%i%s" filename layerSafe extraPart i ext)

// iterates for over a given matrix, each pass highlights what is recongized
let makeStep (net: Net) (inputBlob: Blob) width height (data: float32[]) layer mean =
    time "makeStep" (fun () -> 

        // first add the data to the blob
        inputBlob.SetData(data)

        // get a reference to the bolb of the output layer
        let outputBlob = net.BlobByName(layer)

        for i in 1 .. 10 do
            // first get the blobs data
            let inputData = inputBlob.GetData()

            // adding a random jitter helps the net find aspects of the image
            let xJitter, yJitter = rnd.Next(-32, 32), rnd.Next(-32, 32)
            let inputData = PseudoMatrices.splitRollCombine inputData width height xJitter yJitter
            inputBlob.SetData(inputData)

            // forward to a given layer, set that layers target (the diff)
            // the propergate the data back the inputBlob
            net.ForwardTo(layer) |> ignore
            outputBlob.SetDiff(outputBlob.GetData())
            net.BackwardFrom(layer)

            // the inputBlob's diff now contains what the net has reconized
            let inputData = inputBlob.GetData()
            let inputDiff = inputBlob.GetDiff()

            // apply the reconized data to the image data
            let absMean = inputDiff |> Seq.map Math.Abs |> Seq.average
            let inputData' =
                Seq.zip inputData inputDiff 
                |> Seq.map(fun (data, diff) -> data + (1.5f / absMean * diff))
                |> Seq.toArray

            // undo the roll
            let inputData'' = PseudoMatrices.splitRollCombine inputData' width height -xJitter -yJitter

            // set the data read for the next pass
            inputBlob.SetData(inputData'')

        // at the end of the interatiosn retun the image data
        inputBlob.GetData())


[<EntryPoint>]
let main argv = 
    let modelFile   = argv.[0]
    let trainedFile = argv.[1]
    let meanFile    = argv.[2]

    // unpack the argument for the files to be tested
    let imgFile = argv.[3]

    Console.ReadLine()

    Caffe.SetMode Brew.GPU
    let deviceId = Caffe.FindDevice()
    Caffe.SetDevice deviceId

    time "main" (fun () ->

        // load the net's model and import the train data
        let net = new Net(modelFile, Phase.Train)
        net.CopyTrainedLayersFrom(trainedFile)

        // get the input blob where we'll put the image data
        let inputBlob = net.InputBlobs |> Seq.head

        // load the image bitmap and format it correctly for the net
        let bitmap = Image.FromFile(imgFile) :?> Bitmap
        let bitmapResized = DotNetImaging.resizeBitmap bitmap 1000.
        let allChannels = DotNetImaging.formatBitmapAsBgrChannels bitmapResized
        let size = bitmapResized.Size
        let mean = BlobHelpers.loadMean meanFile numChannels size.Width size.Height
        ArrayHelpers.arraySubInPlace mean allChannels

        // select the target layer, different layers produce different effects
        let layer = "inception_4c/output"
        //let layer = "inception_3b/5x5_reduce"

        // zoom over different levels of the image to help the net find different element
        for zoomFactor in 4 .. -1 .. 2  do

            time (sprintf "zoom %i" zoomFactor) (fun () ->

                // perform the actual zooming
                let zoomWidth, zoomHeight, subMatrices = PseudoMatrices.splitZoomCombine zoomFactor size.Width size.Height allChannels

                // reshape the net to the target image size
                inputBlob.Reshape([|1; numChannels; zoomWidth; zoomHeight |])
                net.Reshape()

                // run the highlight over each zoomed image piece
                let treadedParts =
                    Array.map (fun subMatrix -> 
                        makeStep  net inputBlob zoomWidth zoomHeight subMatrix layer mean) subMatrices 

                // put the image back together and save
                PseudoMatrices.unzoom zoomFactor size.Width size.Height treadedParts allChannels
                let imageToSave = ArrayHelpers.arrayAdd mean allChannels
                DotNetImaging.saveImageDotNet imageToSave (numberedFileName imgFile layer "" zoomFactor) size)

        time "final pass" (fun () ->
            // make a final pass over the unzoomed image
            inputBlob.Reshape([|1; numChannels; size.Width; size.Height |])
            net.Reshape()
            let finalLayer = makeStep  net inputBlob size.Width size.Height allChannels layer mean
            let imageToSave = ArrayHelpers.arrayAdd mean finalLayer
            DotNetImaging.saveImageDotNet imageToSave (numberedFileName imgFile layer "" 0) size))

    0 // return an integer exit code
