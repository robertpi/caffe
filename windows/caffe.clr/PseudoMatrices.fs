namespace Caffe.Clr
open System

module PseudoMatrices =
    let splitChanels (data: 't[])  =
        let chanelSize = data.Length / 3

        let b = data |> Seq.take chanelSize 
        let g = data |> Seq.skip chanelSize |> Seq.take chanelSize 
        let r = data |> Seq.skip (chanelSize * 2)

        b, g, r

    let private getSplitShift shift length =
        if shift > 0 then
            shift
        else
            length + shift

    let rollArrayX xLength shift (data:'t[])  =
        let out:'t[] = Array.zeroCreate data.Length
        let shift = getSplitShift shift xLength 
        for i in 0 .. data.Length - 1 do
            let row = (i / xLength) * xLength
            out.[row + ((i + shift) % xLength) ] <- data.[i]
        out

    let rollArrayY xLength yLength shift (data:'t[]) =
        let shift = getSplitShift shift yLength 
        let out:'t[] = Array.zeroCreate data.Length
        for i in 0 .. data.Length - 1 do
            let shiftIndex = (i + (shift * xLength)) % data.Length
            out.[shiftIndex] <- data.[i]
        out

    let splitRollCombine inputData width height xShift yShift =
        let g,b,r = splitChanels inputData
        let rollArray  =
            (rollArrayX width xShift) >> 
            (rollArrayY width height yShift)
        [| yield! g |> Seq.toArray |> rollArray;
            yield! b |> Seq.toArray |> rollArray;
            yield! r |> Seq.toArray |> rollArray; |]

    let zoom zoomFactor width height (data:'t[]) =
        let zoomedWidth = width / zoomFactor
        let zoomedHeight = height / zoomFactor
        zoomedWidth,
        zoomedHeight,
        [| for y in 0 .. zoomFactor - 1 do
                for x in 0 .. zoomFactor - 1 do
                    let yNext = y + 1
                    let xNext = x + 1
                    yield [| for actualY in zoomedHeight * y .. (zoomedHeight * yNext) - 1 do
                                let hightOffset = width * actualY
                                yield! data.[ hightOffset + (zoomedWidth * x) .. hightOffset + (zoomedWidth * xNext) - 1 ] |] |]


    let splitZoomCombine zoomFactor width height inputData =
        let g,b,r = splitChanels inputData
        let zoomWidth, zoomHeight, g = zoom zoomFactor width height (g |> Seq.toArray)
        let _, _, b = zoom zoomFactor width height (b |> Seq.toArray)
        let _, _, r = zoom zoomFactor width height (r |> Seq.toArray)
        zoomWidth, zoomHeight,
        Array.map3 (fun g b r -> [| yield!  g; yield! b;  yield! r |]) g b r
    
    let unzoom zoomFactor width height (data:seq<'t[]>) (originalMatrix:'t[]) =
        let channelLength = originalMatrix.Length / 3
        let zoomedWidth = width / zoomFactor
        let zoomedHeight = height / zoomFactor
        data |> Seq.iteri(fun i subMatrix ->
                let subX = i % zoomFactor
                let subY = i / zoomFactor
                let g,b,r = splitChanels subMatrix
                let doChannel offset (channel: 't[]) =
                    for y in 0 .. zoomedHeight - 1 do            
                        let offsetX = zoomedWidth * subX
                        let offsetY = width * (subY * zoomedHeight)
                        for x in 0 .. zoomedWidth - 1 do
                            originalMatrix.[ offset + offsetY + (y * width) + offsetX + x  ] <- channel.[(y * zoomedWidth) + x]
                doChannel 0 (g |> Seq.toArray)
                doChannel channelLength (b |> Seq.toArray)
                doChannel (channelLength * 2) (r |> Seq.toArray))


// add these as unit test somewhere
//let test =
//    [| 0. .. 29. |]
//
//
//let printArrayBy x (data:'t[]) =
//    for i in 0 .. data.Length - 1 do
//        printf "%f\t" data.[i]
//        if i % x = x - 1 then printfn ""
//    printfn ""
//
//printArrayBy 5 test
//
//let test' = rollArrayX 5 4 test 
//printArrayBy 5 test'
//
//let test'' = rollArrayX 5 -4 test'
//printArrayBy 5 test''
//
//
//let testy' = rollArrayY 5 6 1 test 
//printArrayBy 5  testy'
//
//let testy'' = rollArrayY 5 6  -1 testy'
//printArrayBy 5 testy''
//
//let test =
//    [| 0. .. 99. |]
//
//let res = zoom 2 10 10 test
//
//for grid in res do
//    printArrayBy 5 grid
//
//unzoom 2 10 10 res test
//
//printArrayBy 10 test
//

