namespace Caffe.Clr

module ArrayHelpers =
    /// adds the given value to each member of the array
    let arrayAddInPlace value (data: float32[]) =
        data
        |> Array.iteri (fun i x -> data.[i] <- (x - value))

    /// subtracts the given value to each member of the array
    let arraySubInPlace value (data: float32[]) =
        arrayAddInPlace -value data

    /// creates a new array, adding the given value to each member of the new array
    let arrayAdd value (data: float32[]) =
        data
        |> Array.map (fun x -> x - value)

    /// creates a new array, subtracting the given value to each member of the new array
    let arraySub value (data: float32[]) =
        arrayAdd -value data

