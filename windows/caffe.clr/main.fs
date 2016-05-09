module MainEntry

open Caffe.Clr
open Caffe.Clr.Interop

let main() =
    let ptr = NetFunctions.caffe_net_new(@"C:\code\mscaffe\models\bvlc_googlenet\deploy.prototxt", Phase.Test)
    printfn "%O" ptr
    ()

main()
