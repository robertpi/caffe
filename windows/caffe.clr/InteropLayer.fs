namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices

module LayerFunctions =
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern bool caffe_layer_ShareInParallel(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern bool caffe_layer_IsShared(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_layer_SetShared(IntPtr layerAnon, bool is_shared)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_layer_Reshape(IntPtr layerAnon, IntPtr bottomAnon, int bottomLength, IntPtr topAnon, int topLength)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_layer_Forward(IntPtr layerAnon, IntPtr bottomAnon, int bottomLength, IntPtr topAnon, int topLength)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_layer_Backward(IntPtr layerAnon, IntPtr topAnon, IntPtr propagate_downAnon, IntPtr bottomAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_layer_blob(IntPtr layerAnon, int i) 

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_blobs_size(IntPtr layerAnon) 

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_layer_loss(IntPtr layerAnon, int top_index)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_layer_set_loss(IntPtr layerAnon, int top_index, float32 value)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_layer_type(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_ExactNumBottomBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_MinBottomBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_MaxBottomBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_ExactNumTopBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_MinTopBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_layer_MaxTopBlobs (IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern bool caffe_layer_EqualNumBottomTopBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern bool caffe_layer_AutoTopBlobs(IntPtr layerAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern bool caffe_layer_AllowForceBackward(IntPtr layerAnon, int bottom_index)
