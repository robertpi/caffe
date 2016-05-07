namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr.Interop

type Layer internal (layerAnon: IntPtr) = 

    member private x.GetIntPtr() =
        layerAnon

    member x.ShareInParallel() =
        LayerFunctions.caffe_layer_ShareInParallel(layerAnon)


    member x.IsShared() =
        LayerFunctions.caffe_layer_IsShared(layerAnon)

    member x.SetShared(is_shared: bool) =
        LayerFunctions.caffe_layer_SetShared(layerAnon, is_shared)

    member x.Reshape(bottom: Blob[], top: Blob[]) =
        let bottomPtrs = bottom |> Array.map (fun x -> x.GetIntPtr())
        let topPtrs = top  |> Array.map (fun x -> x.GetIntPtr())
        let bottomHdl = GCHandle.Alloc(bottomPtrs)
        let topHdl = GCHandle.Alloc(topPtrs)
        LayerFunctions.caffe_layer_Reshape(layerAnon, bottomHdl.AddrOfPinnedObject(), bottomPtrs.Length, topHdl.AddrOfPinnedObject(), topPtrs.Length)
        bottomHdl.Free()
        topHdl.Free()

    member x.Forward(bottom: Blob[], top: Blob[]) =
        let bottomPtrs = bottom |> Array.map (fun x -> x.GetIntPtr())
        let topPtrs = top  |> Array.map (fun x -> x.GetIntPtr())
        let bottomHdl = GCHandle.Alloc(bottomPtrs)
        let topHdl = GCHandle.Alloc(topPtrs)
        let loss = LayerFunctions.caffe_layer_Forward(layerAnon, bottomHdl.AddrOfPinnedObject(), bottomPtrs.Length, topHdl.AddrOfPinnedObject(), topPtrs.Length)
        bottomHdl.Free()
        topHdl.Free()
        loss

    member x.Backward(top: Blob[], propagate_down: bool[], bottom: Blob[]) =
        let topPtrs = top  |> Array.map (fun x -> x.GetIntPtr())
        let bottomPtrs = bottom |> Array.map (fun x -> x.GetIntPtr())
        let topHdl = GCHandle.Alloc(topPtrs)
        let propHdl = GCHandle.Alloc(propagate_down)
        let bottomHdl = GCHandle.Alloc(bottomPtrs)
        LayerFunctions.caffe_layer_Backward(layerAnon, topHdl.AddrOfPinnedObject(), propHdl.AddrOfPinnedObject(), bottomHdl.AddrOfPinnedObject())
        topHdl.Free()
        propHdl.Free()
        bottomHdl.Free()

    member x.blob(i: int) =
        let blobPtr = LayerFunctions.caffe_layer_blob(layerAnon, i) 
        new Blob(blobPtr)

    member x.loss(top_index: int) =
        LayerFunctions.caffe_layer_loss(layerAnon, top_index)

    member x.set_loss(top_index: int, value: float32) =
        LayerFunctions.caffe_layer_set_loss(layerAnon, top_index, value)


    member x.Type() =
        LayerFunctions.caffe_layer_type(layerAnon)


    member x.ExactNumBottomBlobs() =
        LayerFunctions.caffe_layer_ExactNumBottomBlobs(layerAnon)

    member x.MinBottomBlobs() =
        LayerFunctions.caffe_layer_MinBottomBlobs(layerAnon)


    member x.MaxBottomBlobs() =
        LayerFunctions.caffe_layer_MaxBottomBlobs(layerAnon)

    member x.ExactNumTopBlobs() =
        LayerFunctions.caffe_layer_ExactNumTopBlobs(layerAnon)

    member x.MinTopBlobs() =
        LayerFunctions.caffe_layer_MinTopBlobs(layerAnon)

    member x.MaxTopBlobs() =
        LayerFunctions.caffe_layer_MaxTopBlobs (layerAnon)


    member x.EqualNumBottomTopBlobs() =
        LayerFunctions.caffe_layer_EqualNumBottomTopBlobs(layerAnon)


    member x.AutoTopBlobs() =
        LayerFunctions.caffe_layer_AutoTopBlobs(layerAnon)


    member x.AllowForceBackward(bottom_index: int) =
        LayerFunctions.caffe_layer_AllowForceBackward(layerAnon, bottom_index)
