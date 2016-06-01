namespace Caffe.Clr
open System

module PseudoMatrices =
    /// splits a matrix into three equal sized channels
    let splitChannels (data: 't[])  =
        let channelSize = data.Length / 3

        let c1 = data |> Seq.take channelSize 
        let c2 = data |> Seq.skip channelSize |> Seq.take channelSize 
        let c3 = data |> Seq.skip (channelSize * 2)

        c1, c2, c3

    let private getSplitShift shift length =
        if shift > 0 then
            shift
        else
            length + shift

    /// rolls a matrix 'shift' number of places to the right
    let rollArrayX xLength shift (data:'t[])  =
        let out:'t[] = Array.zeroCreate data.Length
        let shift = getSplitShift shift xLength 
        for i in 0 .. data.Length - 1 do
            let row = (i / xLength) * xLength
            out.[row + ((i + shift) % xLength) ] <- data.[i]
        out

    /// rolls a matrix 'shift' number of places downwards
    let rollArrayY xLength yLength shift (data:'t[]) =
        let shift = getSplitShift shift yLength 
        let out:'t[] = Array.zeroCreate data.Length
        for i in 0 .. data.Length - 1 do
            let shiftIndex = (i + (shift * xLength)) % data.Length
            out.[shiftIndex] <- data.[i]
        out

    /// first splits the matrix, then rolls each channel it's X & Y axis
    /// then recombines into a single array
    let splitRollCombine inputData width height xShift yShift =
        let g,b,r = splitChannels inputData
        let rollArray  =
            (rollArrayX width xShift) >> 
            (rollArrayY width height yShift)
        [| yield! g |> Seq.toArray |> rollArray;
           yield! b |> Seq.toArray |> rollArray;
           yield! r |> Seq.toArray |> rollArray; |]

    /// splits a matrix into zoomFactor ^ 2 equal sided sub-sections
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


    // splits a matrix into it's channels, performs a zoom on each
    // channel, then recombines into a single array
    let splitZoomCombine zoomFactor width height inputData =
        let g,b,r = splitChannels inputData
        let zoomWidth, zoomHeight, g = zoom zoomFactor width height (g |> Seq.toArray)
        let _, _, b = zoom zoomFactor width height (b |> Seq.toArray)
        let _, _, r = zoom zoomFactor width height (r |> Seq.toArray)
        zoomWidth, zoomHeight,
        Array.map3 (fun g b r -> [| yield!  g; yield! b;  yield! r |]) g b r

    /// takes sequence of zoomed matrices and recombines then, writing the results
    /// into the given 'originalMatrix'    
    let unzoom zoomFactor width height (data:seq<'t[]>) (originalMatrix:'t[]) =
        let channelLength = originalMatrix.Length / 3
        let zoomedWidth = width / zoomFactor
        let zoomedHeight = height / zoomFactor
        data |> Seq.iteri(fun i subMatrix ->
                let subX = i % zoomFactor
                let subY = i / zoomFactor
                let g,b,r = splitChannels subMatrix
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

