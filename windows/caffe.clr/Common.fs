namespace Caffe.Clr

type Phase =
    | Train = 0
    | Test = 1

// the shape of a layer or blob, can't be an emun
// as we can have an arbitrary number of dimensions
module Shape =
    let Num = 0
    let Channels = 1
    let Width = 2
    let Height = 3
