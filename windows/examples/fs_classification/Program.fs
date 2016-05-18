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
        

        let bitmap = Image.FromFile(file) :?> Bitmap 

        let bitmap = new Bitmap(bitmap, size)

        let numChannels = 3 

        inputBlob.Reshape([|1; numChannels; bitmap.Width; bitmap.Height|])
        net.Reshape()

        let byteData = arrayOfImage bitmap

        meanBlob.Reshape([|1; numChannels; bitmap.Width; bitmap.Height |])
        let mean = meanBlob.GetData()

        let matrix = OpenCV.CalculateMeanMatrix(mean, numChannels, bitmap.Width, bitmap.Height, bitmap.Width, bitmap.Height)
        OpenCV.LoadImage(file, inputBlob, matrix)

        let data = inputBlob.GetData()

        let loss = ref 0.0f
        net.Forward(loss)


        let output = net.OutputBlobs |> Seq.head

        let labels = File.ReadAllLines(labelFile)
        let resultChannels = output.Shape(Shape_Channels)

        //assert (labels.Length = resultChannels)

        let resultData = output.GetData()

        let results =
            Seq.zip resultData labels
            |> Seq.sortByDescending fst
            |> Seq.take 10

        for (percent, label) in results do
            printfn "%f %s" percent label

        0