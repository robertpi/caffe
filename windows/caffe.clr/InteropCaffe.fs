namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices


module CaffeFunctions =

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr caffe_ReadProtoFromBinaryFileOrDie(string filename)