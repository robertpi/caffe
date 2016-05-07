namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices


module BlobFunctions =

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_blob_new(IntPtr shape, int length)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_Reshape(IntPtr blobAnon, IntPtr shape, int length)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_ReshapeLike(IntPtr blobAnon, IntPtr otherAnon)

// TODO another bloody vector
//  inline const vector<int>& shape() const { return shape_; }

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_shape(IntPtr blobAnon, int index)


    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_num_axes(IntPtr blobAnon)


    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_count(IntPtr blobAnon) 

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_count_start_end(IntPtr blobAnon, int start_axis, int end_axis)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_count_start(IntPtr blobAnon, int start_axis)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_CanonicalAxisIndex(IntPtr blobAnon, int axis_index)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_offset(IntPtr blobAnon, int n, int c, int h, int w)


    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_blob_offset_vector(IntPtr blobAnon, IntPtr indices, int length)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_data_at(IntPtr blobAnon, int n, int c, int h, int w)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_diff_at(IntPtr blobAnon, int n, int c, int h, int w)


    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_data_at_vector(IntPtr blobAnon, IntPtr index, int length)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_diff_at_vector(IntPtr blobAnon, IntPtr index, int length)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_cpu_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_set_cpu_data(IntPtr blobAnon, float32[] data)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int[] caffe_blob_gpu_shape(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_gpu_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_cpu_diff(IntPtr blobAnon)


    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_gpu_diff(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_mutable_cpu_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_mutable_gpu_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_mutable_cpu_diff(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32[] caffe_blob_mutable_gpu_diff(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_Update(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_asum_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_asum_diff(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_sumsq_data(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_blob_sumsq_diff(IntPtr blobAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_scale_data(IntPtr blobAnon, float32 scale_factor)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_blob_scale_diff(IntPtr blobAnon, float32 scale_factor)