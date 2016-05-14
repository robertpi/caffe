namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr.Interop

type Blob internal (blobAnon: IntPtr) = 

    do if blobAnon = IntPtr.Zero then failwith "blobAnon must not be null"

    static member FromShape(shape: int[]) =
        let shapeHdl = GCHandle.Alloc(shape, GCHandleType.Pinned)
        let blobPtr = BlobFunctions.caffe_blob_new(shapeHdl.AddrOfPinnedObject(), shape.Length)
        shapeHdl.Free()
        new Blob(blobPtr)

    member internal x.GetIntPtr() =
        blobAnon

    member x.Reshape(shape: int[]) =
        let shapeHdl = GCHandle.Alloc(shape, GCHandleType.Pinned)
        let blobPtr = BlobFunctions.caffe_blob_Reshape(blobAnon, shapeHdl.AddrOfPinnedObject(), shape.Length)
        shapeHdl.Free()

    member x.ReshapeLike(other: Blob) =
        let otherPtr = other.GetIntPtr()
        BlobFunctions.caffe_blob_ReshapeLike(blobAnon, otherPtr)

    member x.Shape
        with get (index: int) =
            BlobFunctions.caffe_blob_shape(blobAnon, index)

    member x.NumberOfAxes() =
        BlobFunctions.caffe_blob_num_axes(blobAnon)

    member x.Count
        with get() =
            BlobFunctions.caffe_blob_count(blobAnon) 

    member x.CountStartEnd(start_axis: int, end_axis: int) =
        BlobFunctions.caffe_blob_count_start_end(blobAnon, start_axis, end_axis)

    member x.countStart(start_axis: int) =
        BlobFunctions.caffe_blob_count_start(blobAnon, start_axis)

    member x.CanonicalAxisIndex(axis_index: int) =
        BlobFunctions.caffe_blob_CanonicalAxisIndex(blobAnon, axis_index)

    member x.Offset(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_offset(blobAnon, n, c, h, w)

    member x.Offset(indices: int[]) =
        let indicesHdl = GCHandle.Alloc(indices, GCHandleType.Pinned)
        let offset = BlobFunctions.caffe_blob_offset_vector(blobAnon, indicesHdl.AddrOfPinnedObject(), indices.Length)
        indicesHdl.Free()
        offset

    member x.DataAt(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_data_at(blobAnon, n, c, h, w)

    member x.DataAt(index: int[]) =
        let indexHdl = GCHandle.Alloc(index, GCHandleType.Pinned)
        let data = BlobFunctions.caffe_blob_data_at_vector(blobAnon, indexHdl.AddrOfPinnedObject(), index.Length)
        indexHdl.Free()
        data

    member x.DiffAt(n: int, c: int, h: int, w: int) =
        BlobFunctions.caffe_blob_diff_at(blobAnon, n, c, h, w)

    member x.DiffAt(index: int[]) =
        let indexHdl = GCHandle.Alloc(index, GCHandleType.Pinned)
        let data = BlobFunctions.caffe_blob_diff_at_vector(blobAnon, indexHdl.AddrOfPinnedObject(), index.Length)
        indexHdl.Free()
        data

    member x.GetData() =
        let ptr = BlobFunctions.caffe_blob_cpu_data(blobAnon)
        let c = x.Count
        let result: float32[] = Array.zeroCreate (c)
        Marshal.Copy(ptr, result, 0, result.Length)
        result

    member x.SetData (value: float32[]) =
        let ptr = BlobFunctions.caffe_blob_mutable_cpu_data(blobAnon)
        Marshal.Copy(value, 0, ptr, x.Count)

    member x.GetDiff() =
        let ptr = BlobFunctions.caffe_blob_cpu_diff(blobAnon)
        let result: float32[] = Array.zeroCreate (x.Count)
        Marshal.Copy(ptr, result, 0, result.Length)
        result

    member x.SetDiff(value: float32[]) =
        let ptr = BlobFunctions.caffe_blob_mutable_cpu_diff(blobAnon)
        Marshal.Copy(value, 0, ptr, x.Count)

    // made some members private as I don't think they're necessary
    // and won't work correctly yet
    member private x.cpu_data() =
        let ptr = BlobFunctions.caffe_blob_cpu_data(blobAnon)
        let result: float32[] = Array.zeroCreate (x.Count)
        Marshal.Copy(ptr, result, 0, result.Length)
        result

    member private x.set_cpu_data(data: float32[]) =
        // should copy the data?
        let dataHdl = GCHandle.Alloc(data, GCHandleType.Pinned)
        BlobFunctions.caffe_blob_set_cpu_data(blobAnon, dataHdl.AddrOfPinnedObject())
        dataHdl.Free()

    member private x.gpu_shape() =
        BlobFunctions.caffe_blob_gpu_shape(blobAnon)

    member private x.gpu_data() =
        BlobFunctions.caffe_blob_gpu_data(blobAnon)

    member x.cpu_diff() =
        BlobFunctions.caffe_blob_cpu_diff(blobAnon)

    member private x.gpu_diff() =
        BlobFunctions.caffe_blob_gpu_diff(blobAnon)

    member private x.mutable_cpu_data() =
        BlobFunctions.caffe_blob_mutable_cpu_data(blobAnon)


    member private x.mutable_gpu_data() =
        BlobFunctions.caffe_blob_mutable_gpu_data(blobAnon)


    member private x.mutable_cpu_diff() =
        BlobFunctions.caffe_blob_mutable_cpu_diff(blobAnon)


    member private x.mutable_gpu_diff() =
        BlobFunctions.caffe_blob_mutable_gpu_diff(blobAnon)


    member x.Update() =
        BlobFunctions.caffe_blob_Update(blobAnon)


    member x.AsumData() =
        BlobFunctions.caffe_blob_asum_data(blobAnon)


    member x.AsumDiff() =
        BlobFunctions.caffe_blob_asum_diff(blobAnon)


    member x.SumSqData() =
        BlobFunctions.caffe_blob_sumsq_data(blobAnon)


    member x.SumSqDiff() =
        BlobFunctions.caffe_blob_sumsq_diff(blobAnon)


    member x.ScaleData(scale_factor: float32) =
        BlobFunctions.caffe_blob_scale_data(blobAnon, scale_factor)


    member x.ScaleDiff(scale_factor: float32) =
        BlobFunctions.caffe_blob_scale_diff(blobAnon, scale_factor)

    member x.FromProto(blobProto: BlobProto, reshape: bool) =
        BlobFunctions.caffe_blob_FromProto(blobAnon, blobProto.GetIntPtr(), reshape)

    static member FromProtoFile(blobProtoFile: string) =
        let ptr = BlobFunctions.caffe_blob_new_FromProto(blobProtoFile)

        new Blob(ptr)