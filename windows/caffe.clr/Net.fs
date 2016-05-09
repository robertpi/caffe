namespace Caffe.Clr
open System
open Caffe.Clr.Interop

type Net(netFile: string, phase: Phase) = 
    let netAnon = NetFunctions.caffe_net_new(netFile, phase)

    member x.LayerByName(layer_name: string) =
        let layerPtr = NetFunctions.caffe_net_layer_by_name(netAnon, layer_name)
        new Layer(layerPtr)

    member x.InputBlob(i: int) =
        let blobPtr = NetFunctions.caffe_net_input_blob(netAnon, i)
        new Blob(blobPtr)

    member x.Forward(loss: byref<float32>) =
        NetFunctions.caffe_net_Forward(netAnon, &loss)

    member x.ForwardFromTo(start: int, ``end``: int) =
        NetFunctions.caffe_net_ForwardFromTo(netAnon, start, ``end``)

    member x.ForwardFrom(start: int) =
        NetFunctions.caffe_net_ForwardFrom(netAnon, start)

    member x.ForwardTo(``end``: int) =
        NetFunctions.caffe_net_ForwardTo(netAnon, ``end``)

    member x.ClearParamDiffs() =
        NetFunctions.caffe_net_ClearParamDiffs(netAnon)

    member x.Backward() =
        NetFunctions.caffe_net_Backward(netAnon)

    member x.BackwardFromTo(start: int, ``end``: int) =
        NetFunctions.caffe_net_BackwardFromTo(netAnon, start, ``end``)

    member x.BackwardFrom(start: int) =
        NetFunctions.caffe_net_BackwardFrom(netAnon, start)

    member x.BackwardTo(``end``: int) =
        NetFunctions.caffe_net_BackwardTo(netAnon, ``end``)

    member x.Reshape() =
        NetFunctions.caffe_net_Reshape(netAnon)

    member x.ForwardBackward() =
        NetFunctions.caffe_net_ForwardBackward(netAnon)

    member x.Update() =
        NetFunctions.caffe_net_Update(netAnon)

    member x.ShareWeights() =
        NetFunctions.caffe_net_ShareWeights(netAnon)

    member x.CopyTrainedLayersFrom(trained_filename: string) =
        NetFunctions.caffe_net_CopyTrainedLayersFrom(netAnon, trained_filename)

    member x.CopyTrainedLayersFromBinaryProto(trained_filename: string) =
        NetFunctions.caffe_net_CopyTrainedLayersFromBinaryProto(netAnon, string trained_filename)

    member x.CopyTrainedLayersFromHDF5(trained_filename: string) =
        NetFunctions.caffe_net_CopyTrainedLayersFromHDF5(netAnon, trained_filename)

    member x.ToHDF5(filename, write_diff) =
        NetFunctions.caffe_net_ToHDF5(netAnon, filename, write_diff)

    member x.Name
        with get() =
            NetFunctions.caffe_net_name(netAnon)

    member x.LayerName(i: int) =
        NetFunctions.caffe_net_layer_name(netAnon, i)

    member x.BlobName(i: int) =
        NetFunctions.caffe_net_blob_name(netAnon, i)

    member x.Blob(i: int) =
        let blobPtr = NetFunctions.caffe_net_blob(netAnon, int i)
        new Blob(blobPtr)

    member x.Layer(i: int) =
        let layerPtr = NetFunctions.caffe_net_layer(netAnon, int i)
        new Layer(layerPtr)

    member x.Phase
        with get() =
            match NetFunctions.caffe_net_phase(netAnon) with
            | 0 -> Phase.Train
            | 1 -> Phase.Test
            | _ -> failwith "unknown phase value"
