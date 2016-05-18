namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr.Interop

type Net(netFile: string, phase: Phase) = 
    let netAnon = NetFunctions.caffe_net_new(netFile, phase)

    let getBlob i = 
        let blobPtr = NetFunctions.caffe_net_blob(netAnon, i)
        new Blob(blobPtr)
    let getBlobsSize() = NetFunctions.caffe_net_blobs_size(netAnon)
    let blobs = new UnmanagedCollection<Blob>(getBlob, getBlobsSize)

    let getLayer i = 
        let layerPtr = NetFunctions.caffe_net_layer(netAnon, i)
        new Layer(layerPtr)
    let getLayersSize() = NetFunctions.caffe_net_layers_size(netAnon)
    let layers = new UnmanagedCollection<Layer>(getLayer, getLayersSize)

    let getInputBlob i = 
        let layerPtr = NetFunctions.caffe_net_input_blob(netAnon, i)
        new Blob(layerPtr)
    let getInputBlobsSize() = NetFunctions.caffe_net_input_blobs_size(netAnon)
    let inputBlobs = new UnmanagedCollection<Blob>(getInputBlob, getInputBlobsSize)

    let getOutputBlob i = 
        let layerPtr = NetFunctions.caffe_net_output_blob(netAnon, i)
        new Blob(layerPtr)
    let getOutputBlobsSize() = NetFunctions.caffe_net_output_blobs_size(netAnon)
    let outputBlobs = new UnmanagedCollection<Blob>(getOutputBlob, getOutputBlobsSize)

    let getLayerName i = 
        let ptr = NetFunctions.caffe_net_layer_name(netAnon, i)
        Common.MarshalString (ptr)
    let layerNames = new UnmanagedCollection<string>(getLayerName, getLayersSize)

    let getBlobName i = 
        let ptr = NetFunctions.caffe_net_blob_name(netAnon, i)
        Common.MarshalString (ptr)
    let blobNames = new UnmanagedCollection<string>(getBlobName, getLayersSize)

    member x.LayerByName(layer_name: string) =
        let layerPtr = NetFunctions.caffe_net_layer_by_name(netAnon, layer_name)
        new Layer(layerPtr)

    member x.InputBlobs = inputBlobs

    member x.OutputBlobs = outputBlobs

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
            let ptr = NetFunctions.caffe_net_name(netAnon)
            Common.MarshalString (ptr)

    member x.LayerNames = layerNames

    member x.BlobNames = blobNames

    member x.Blobs = blobs

    member x.Layers = layers

    member x.Phase
        with get() =
            match NetFunctions.caffe_net_phase(netAnon) with
            | 0 -> Phase.Train
            | 1 -> Phase.Test
            | _ -> failwith "unknown phase value"
