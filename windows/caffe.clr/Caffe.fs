namespace Caffe.Clr

open Caffe.Clr.Interop

type Brew =
    | CPU = 0
    | GPU = 1

module Caffe =

    let ReadProtoFromBinaryFileOrDie filename =
        let ptr = CaffeFunctions.caffe_ReadProtoFromBinaryFileOrDie(filename)
        new BlobProto(ptr)

    let SetMode (mode: Brew) =
        CaffeFunctions.caffe_set_mode(int mode)

    let SetDevice deviceId =
        CaffeFunctions.caffe_SetDevice deviceId

    let FindDevice deviceId =
        CaffeFunctions.caffe_FindDevice()
