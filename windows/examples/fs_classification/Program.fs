namespace Caffe.Clr.Examples
module Classification =

    open System.IO
    open System.Drawing
    open System.Drawing.Imaging
    open System.Runtime.InteropServices
    open Caffe.Clr

    let loadImageDotNet file (size: Size) meanFile (inputBlob: Blob) numChannels =
        let bitmap = Image.FromFile(file) :?> Bitmap

        let bitmap = new Bitmap(bitmap, size)

        let allChannels = DotNetImaging.formatBitmapAsBgrChannels bitmap

        let mean = BlobHelpers.loadMean meanFile numChannels size.Width size.Height 

        ArrayHelpers.arrayAddInPlace mean allChannels

        inputBlob.SetData(allChannels)

    let loadModel modelFile trainedFile =
        // load the network and training data
        let net = new Net(modelFile, Phase.Test)
        net.CopyTrainedLayersFrom(trainedFile)

        // check the network has the right number of inputs/outputs
        assert (net.InputBlobs.Count = 1)
        assert (net.OutputBlobs.Count = 1)

        net 

    let printOutput (net: Net) labelFile =
        // grab a reference to the output blob
        let output = net.OutputBlobs |> Seq.head

        // read the labels file
        let labels = File.ReadAllLines(labelFile)

        // check the network has the same number of outputs as the labels file
        let resultChannels = output.Shape(Shape.Channels)
        assert (labels.Length = resultChannels)

        // get the output data
        let resultData = output.GetData()

        // match the results to labels and sort
        let results =
            Seq.zip resultData labels
            |> Seq.sortByDescending fst
            |> Seq.take 10

        // print the top 10 results
        for (percent, label) in results do
            printfn "%f %s" percent label

    let loadImageIntoBlob file (size: Size) meanFile (inputBlob: Blob) numChannels =
        let img = OpenCV.ImageRead(file, -1)

        // resize the image if necessary
        let imgResized =
            if img.Width = size.Width && img.Height = size.Height then 
                img
            else
                OpenCV.Resize(img, size.Width, size.Height)

        let sampleFloat = imgResized.ConvertTo(RTypes.CV_32FC3)


        // load the mean blob, calculated from the training data
        let meanBlob = Blob.FromProtoFile(meanFile)

        // check the mean blob has some number of channels as the input blob
        assert (meanBlob.Shape(Shape.Channels) = inputBlob.Shape(Shape.Channels))

        // calculate an overall mean from the mean blob and create a matrix from this
        let mean = meanBlob.GetData()
        let meanMatrix = OpenCV.MergeFloatArray(mean, numChannels, meanBlob.Shape(Shape.Width), meanBlob.Shape(Shape.Height))
        let mean = OpenCV.Mean(meanMatrix)
        let meanMatrix = Matrix.FromScalar(size.Height, size.Width, meanMatrix.Type(), mean)

        // subtract the mean matrix from the sample float
        let sampleNormalized = OpenCV.Subtract(sampleFloat, meanMatrix)

        // load normalized values into the input blob
        OpenCV.SplitToInputBlob(sampleNormalized, inputBlob)

    [<EntryPoint>]
    let main argv = 
        // unpack arguments related to the model
        let modelFile   = argv.[0]
        let trainedFile = argv.[1]
        let meanFile    = argv.[2]
        let labelFile   = argv.[3]

        // unpack the argument for the files to be tested
        let file = argv.[4]

        let numChannels = 3 

        let net = loadModel modelFile trainedFile 

        // grab a reference to the input blob where will put the image
        let inputBlob = net.InputBlobs |> Seq.head

        let size = new Size(inputBlob.Shape(Shape.Width), inputBlob.Shape(Shape.Height))

        // reshapre the input blob and network to suite out input image
        inputBlob.Reshape([|1; numChannels; size.Width; size.Height|])
        net.Reshape()

        let useDotNet = true

        if useDotNet then
            loadImageDotNet file size meanFile inputBlob numChannels
        else
            loadImageIntoBlob file size meanFile inputBlob numChannels

        // forward the data though the network
        let loss = ref 0.0f
        net.Forward(loss)

        printOutput net labelFile

        // exit zero
        0