namespace Caffe.Clr

module ArrayHelpers =

    let arrayAddInPlace value (data: float32[]) =
        data
        |> Array.iteri (fun i x -> data.[i] <- (x - value))

    let arraySubInPlace value (data: float32[]) =
        arrayAddInPlace -value data

    let arrayAdd value (data: float32[]) =
        data
        |> Array.map (fun x -> x - value)

    let arraySub value (data: float32[]) =
        arrayAdd -value data

