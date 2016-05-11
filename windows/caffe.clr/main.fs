module MainEntry

open Caffe.Clr
open Caffe.Clr.Interop

let main() =
    let net = new Net(@"C:\code\mscaffe\models\bvlc_googlenet\deploy.prototxt", Phase.Test)

    printfn "%s" net.Name

    for layerName in net.LayerNames do
        printfn "%s" layerName
        let layer = net.LayerByName(layerName)
        printfn "%s" layer.Type

//    for blob in net.Blobs do
//        printfn "%s" blob.

main()
