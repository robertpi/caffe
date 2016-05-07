namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr.Interop

type Blob internal (blobAnon: IntPtr) = 

    static member FromShape(shape: int[]) =
        let shapeHdl = GCHandle.Alloc(shape)
        let blobPtr = BlobFunctions.caffe_blob_new(shapeHdl.AddrOfPinnedObject(), shape.Length)
        shapeHdl.Free()
        new Blob(blobPtr)

    member internal x.GetIntPtr() =
        blobAnon

    member x.Reshape(shape: int[]) =
        let shapeHdl = GCHandle.Alloc(shape)
        let blobPtr = BlobFunctions.caffe_blob_Reshape(blobAnon, shapeHdl.AddrOfPinnedObject(), shape.Length)
        shapeHdl.Free()

    member x.ReshapeLike(other: Blob) =
        let otherPtr = other.GetIntPtr()
        BlobFunctions.caffe_blob_ReshapeLike(blobAnon, otherPtr)

    member x.shape(index: int) =
        BlobFunctions.caffe_blob_shape(blobAnon, index)

    member x.num_axes() =
        BlobFunctions.caffe_blob_num_axes(blobAnon)

    member x.count() =
        BlobFunctions.caffe_blob_count(blobAnon) 

    member x.count_start_end(start_axis: int, end_axis: int) =
        BlobFunctions.caffe_blob_count_start_end(blobAnon, start_axis, end_axis)

    member x.count_start(start_axis: int) =
        BlobFunctions.caffe_blob_count_start(blobAnon, start_axis)

    member x.CanonicalAxisIndex(axis_index: int) =
        BlobFunctions.caffe_blob_CanonicalAxisIndex(blobAnon, axis_index)

    member x.offset(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_offset(blobAnon, n, c, h, w)

    member x.offset(indices: int[]) =
        let indicesHdl = GCHandle.Alloc(indices)
        let offset = BlobFunctions.caffe_blob_offset_vector(blobAnon, indicesHdl.AddrOfPinnedObject(), indices.Length)
        indicesHdl.Free()
        offset

    member x.data_at(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_data_at(blobAnon, n, c, h, w)

    member x.data_at(index: int[]) =
        let indexHdl = GCHandle.Alloc(index)
        let data = BlobFunctions.caffe_blob_data_at_vector(blobAnon, indexHdl.AddrOfPinnedObject(), index.Length)
        indexHdl.Free()
        data

    member x.diff_at(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_diff_at(blobAnon, n, c, h, w)

    member x.diff_at(index: int[]) =
        let indexHdl = GCHandle.Alloc(index)
        let data = BlobFunctions.caffe_blob_diff_at_vector(blobAnon, indexHdl.AddrOfPinnedObject(), index.Length)
        indexHdl.Free()
        data

    member x.cpu_data() =
        BlobFunctions.caffe_blob_cpu_data(blobAnon)

    member x.set_cpu_data(data: float32[]) =
        BlobFunctions.caffe_blob_set_cpu_data(blobAnon, data)

    member x.gpu_shape() =
        BlobFunctions.caffe_blob_gpu_shape(blobAnon)

    member x.gpu_data() =
        BlobFunctions.caffe_blob_gpu_data(blobAnon)

    member x.cpu_diff() =
        BlobFunctions.caffe_blob_cpu_diff(blobAnon)

    member x.gpu_diff() =
        BlobFunctions.caffe_blob_gpu_diff(blobAnon)

    member x.mutable_cpu_data() =
        BlobFunctions.caffe_blob_mutable_cpu_data(blobAnon)


    member x.mutable_gpu_data() =
        BlobFunctions.caffe_blob_mutable_gpu_data(blobAnon)


    member x.mutable_cpu_diff() =
        BlobFunctions.caffe_blob_mutable_cpu_diff(blobAnon)


    member x.mutable_gpu_diff() =
        BlobFunctions.caffe_blob_mutable_gpu_diff(blobAnon)


    member x.blob_Update() =
        BlobFunctions.caffe_blob_Update(blobAnon)


    member x.asum_data() =
        BlobFunctions.caffe_blob_asum_data(blobAnon)


    member x.asum_diff() =
        BlobFunctions.caffe_blob_asum_diff(blobAnon)


    member x.sumsq_data() =
        BlobFunctions.caffe_blob_sumsq_data(blobAnon)


    member x.sumsq_diff() =
        BlobFunctions.caffe_blob_sumsq_diff(blobAnon)


    member x.scale_data(scale_factor: float32) =
        BlobFunctions.caffe_blob_scale_data(blobAnon, scale_factor)


    member x.scale_diff(scale_factor: float32) =
        BlobFunctions.caffe_blob_scale_diff(blobAnon, scale_factor)