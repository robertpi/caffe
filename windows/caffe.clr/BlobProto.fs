namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr.Interop

type BlobProto internal (blobProtoAnon: IntPtr) = 

    do if blobProtoAnon = IntPtr.Zero then failwith "blobAnon must not be null"

    member internal x.GetIntPtr() =
        blobProtoAnon
