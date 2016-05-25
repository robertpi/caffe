namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices
open Caffe.Clr

module NetFunctions =
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_new(string netFile, Phase phase);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_layer_by_name(IntPtr netAnon, string layer_name);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_blob_by_name(IntPtr netAnon, string blob_name);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_input_blob(IntPtr netAnon, int i);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_net_input_blobs_size(IntPtr netAnon);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_output_blob(IntPtr netAnon, int i);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_net_output_blobs_size(IntPtr netAnon);

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_Forward(IntPtr netAnon, float32& loss)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_net_ForwardFromTo(IntPtr netAnon, int start, int ``end``)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_net_ForwardFrom(IntPtr netAnon, int start)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_net_ForwardTo(IntPtr netAnon, int ``end``)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_ClearParamDiffs(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_Backward(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_BackwardFromTo(IntPtr netAnon, int start, int ``end``)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_BackwardFrom(IntPtr netAnon, int start)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_BackwardTo(IntPtr netAnon, int ``end``)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_Reshape(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern float32 caffe_net_ForwardBackward(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_Update(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_ShareWeights(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_CopyTrainedLayersFrom(IntPtr netAnon, string trained_filename)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_CopyTrainedLayersFromBinaryProto(IntPtr netAnon, string trained_filename)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_CopyTrainedLayersFromHDF5(IntPtr netAnon, string trained_filename)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_net_ToHDF5(IntPtr netAnon, string filename, bool write_diff)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_name(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_layer_name(IntPtr netAnon, int i)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_blob_name(IntPtr netAnon, int i)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_blob(IntPtr netAnon, int i)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_net_blobs_size(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_net_layer(IntPtr netAnon, int i)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_net_layers_size(IntPtr netAnon)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_net_phase(IntPtr netAnon)
