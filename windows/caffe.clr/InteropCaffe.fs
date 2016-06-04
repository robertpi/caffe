namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices


module CaffeFunctions =

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_ReadProtoFromBinaryFileOrDie(string filename)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_set_mode(int mode)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void caffe_SetDevice(int device)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int caffe_FindDevice()