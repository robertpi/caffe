namespace Caffe.Clr.Interop
open System.Runtime.InteropServices

module Common =
    [<Literal>]
    let LibraryName = "caffe.flat.dll"

    let MarshalString = Marshal.PtrToStringAnsi
