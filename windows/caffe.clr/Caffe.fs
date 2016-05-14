namespace Caffe.Clr

open Caffe.Clr.Interop

module Caffe =

    let ReadProtoFromBinaryFileOrDie filename =
        let ptr = CaffeFunctions.caffe_ReadProtoFromBinaryFileOrDie(filename)
        new BlobProto(ptr)
