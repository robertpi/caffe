namespace Caffe.Clr

open System.Collections
open System.Collections.Generic

type UnmanagedCollection<'T>(itemFunc: int -> 'T, sizeFunc: unit -> int) =
    let getEnumerable() =
        seq { for i in 0 .. sizeFunc() - 1 do
                yield itemFunc i }
    interface IList<'T> with
        member this.Item 
            with get(index) = itemFunc index
            and  set(index)(value) = failwith "Read-only collection, setting not supported"
        member this.IndexOf item = getEnumerable() |> Seq.findIndex (fun x -> (x :> obj).Equals item)
        member this.Insert(index, item) = failwith "Read-only collection, insertion not supported"
        member this.RemoveAt(index) = failwith "Read-only collection, removal not supported"
        member this.Count with get() = sizeFunc()
        member this.IsReadOnly with get() = true
        member this.Add(item) = failwith "Read-only collection, addition not supported"
        member this.Clear() = failwith "Read-only collection, clearing not supported"
        member this.Contains(item) = getEnumerable() |> Seq.exists (fun x -> (x :> obj).Equals item)
        member this.CopyTo(target, index) = 
            let items = getEnumerable() |> Array.ofSeq
            items.CopyTo(target, index)
        member this.Remove(item) = failwith "Read-only collection, removal not supported"
        member this.GetEnumerator() = getEnumerable().GetEnumerator() :> IEnumerator<'T>
        member this.GetEnumerator() = getEnumerable().GetEnumerator() :> IEnumerator 

