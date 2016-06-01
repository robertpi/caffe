namespace Caffe.Clr

module BlobHelpers =
    /// loads a mean file, resizes it then caculates the mean value
    let loadMean meanFile numChannels width height =
        let meanBlob = Blob.FromProtoFile(meanFile)

        // not quite sure what the point of this resize is as we're
        // going to calculate an average, but the C++ sample did it!
        meanBlob.Reshape([|1; numChannels; width; height|])
        meanBlob.GetData() |> Seq.average


