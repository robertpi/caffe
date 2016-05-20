namespace Caffe.Clr.Examples
module Classification =

    open System.IO
    open System.Drawing
    open System.Drawing.Imaging
    open System.Runtime.InteropServices
    open Caffe.Clr

    let Shape_Num = 0
    let Shape_Channels = 1
    let Shape_Width = 2
    let Shape_Height = 3

    let CV_32FC3 = 21

    [<EntryPoint>]
    let main argv = 
        let modelFile   = argv.[0]
        let trainedFile = argv.[1]
        let meanFile    = argv.[2]
        let labelFile   = argv.[3]

        let file = argv.[4];

        let net = new Net(modelFile, Phase.Test)
        net.CopyTrainedLayersFrom(trainedFile)

        assert (net.InputBlobs.Count = 1)
        assert (net.OutputBlobs.Count = 1)

        let inputBlob = net.InputBlobs |> Seq.head
        
        let meanBlob = Blob.FromProtoFile(meanFile)

        assert (meanBlob.Shape(Shape_Channels) = inputBlob.Shape(Shape_Channels))

        let size = new Size(inputBlob.Shape(Shape_Width), inputBlob.Shape(Shape_Height))
        

        let img = OpenCV.ImageRead(file, -1)

        // TODO don't resize if we're the same size
        let imgResized = OpenCV.Resize(img, size.Width, size.Height)

        let sampleFloat = imgResized.ConvertTo(CV_32FC3)

        let numChannels = 3 

        let mean = meanBlob.GetData()
        let meanMatrix = OpenCV.MergeFloatArray(mean, numChannels, meanBlob.Shape(Shape_Width), meanBlob.Shape(Shape_Height))
        let mean = OpenCV.Mean(meanMatrix)
        let meanMatrix = Matrix.FromDimensions(size.Width, size.Height, meanMatrix.Type(), mean)


        let sampleNormalized = OpenCV.Subtract(sampleFloat, meanMatrix)

        OpenCV.SplitToInputBlob(sampleNormalized, inputBlob)

        inputBlob.Reshape([|1; numChannels; size.Width; size.Height|])
        net.Reshape()


        let loss = ref 0.0f
        net.Forward(loss)


        let output = net.OutputBlobs |> Seq.head

        let labels = File.ReadAllLines(labelFile)
        let resultChannels = output.Shape(Shape_Channels)

        assert (labels.Length = resultChannels)

        let resultData = output.GetData()

        let results =
            Seq.zip resultData labels
            |> Seq.sortByDescending fst
            |> Seq.take 10

        for (percent, label) in results do
            printfn "%f %s" percent label

        0