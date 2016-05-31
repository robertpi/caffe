namespace Caffe.Clr
open System
open System.Drawing
open System.Drawing.Imaging
open System.Runtime.InteropServices

type internal Edge =
    | Width
    | Height

module DotNetImaging =
    let private arrayOfImage (bitmap: Bitmap) =
        let bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat)
        try
            let numbytes = bmpdata.Stride * bitmap.Height
            let bytedata: byte[] = Array.zeroCreate numbytes
            let ptr = bmpdata.Scan0;

            Marshal.Copy(ptr, bytedata, 0, numbytes)

            bytedata
        finally
            bitmap.UnlockBits(bmpdata)

    let formatBitmapAsBgrChannels (bitmap: Bitmap) =
        let byteData = arrayOfImage bitmap
        let r =
            seq { for i in 0 .. 4 .. byteData.Length - 1 do
                    yield float32 byteData.[i] }
        let g =
            seq { for i in 0 .. 4 .. byteData.Length - 1 do
                    yield float32 byteData.[i + 1] }
        let b =
            seq { for i in 0 .. 4 .. byteData.Length - 1 do
                    yield float32 byteData.[i + 2] }
        let rgb =
            seq { yield! r; yield! g; yield! b }

        rgb |> Seq.toArray



    let resizeBitmap (bitmap: Bitmap) longEdgeMax =

        let longEdge, edge = 
            if bitmap.Width > bitmap.Height then bitmap.Width |> float, Width
            else bitmap.Height |> float, Height
            
        let longEdgeTarget = min longEdge longEdgeMax 
        let scaleRatio = longEdgeTarget / longEdge 

        let width, height =
            match edge with
            | Width -> longEdgeTarget, scaleRatio * float bitmap.Height
            | Height -> scaleRatio * float bitmap.Width, longEdgeTarget

        let size = new Size(int width, int height)
        new Bitmap(bitmap, size)

    let saveImageDotNet (data: float32[]) file (size: Size) =
        let bitmap = new Bitmap(size.Width, size.Height)

        let b, g, r = PseudoMatrices.splitChanels data

        let rgb = Seq.zip3 r g b 

        rgb |> Seq.iteri (fun i (r, g, b)  ->
            let x = i % size.Width
            let y = i / size.Width
            let intMax255 x =
                max (min (int x) 255) 0
            let p = Color.FromArgb(intMax255 r, intMax255 g, intMax255 b)
            bitmap.SetPixel(x, y, p))

        bitmap.Save(file)

