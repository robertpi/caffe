namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr
open Caffe.Clr.Interop

type Matrix(matrixAnon: IntPtr) =
     member internal x.GetIntPtr() = matrixAnon

     member x.Width 
        with get() = OpenCVFunctions.opencv_matrix_width(matrixAnon)

     member x.Height 
        with get() = OpenCVFunctions.opencv_matrix_height(matrixAnon)

     member x.ConvertTo(rtype: int) =
        let ptr = OpenCVFunctions.opencv_matrix_convertTo(matrixAnon, rtype)
        new Matrix(ptr)

module OpenCV =
    let ImageRead(file: string, i: int) =
        let ptr = OpenCVFunctions.opencv_imread(file, i)
        new Matrix(ptr)

    let Resize(m: Matrix, w: int, h: int) =
        let ptr = OpenCVFunctions.opencv_resize(m.GetIntPtr(), w, h)
        new Matrix(ptr)

    let Subtract(mX: Matrix, mY: Matrix) =
        let ptr = OpenCVFunctions.opencv_subtract(mX.GetIntPtr(), mY.GetIntPtr())
        new Matrix(ptr)

    let SplitToInputBlob(inputSample: Matrix, blob: Blob) =
        OpenCVFunctions.opencv_split_to_input_blob(inputSample.GetIntPtr(), blob.GetIntPtr())

    let LoadImage(imgFile: string, blob: Blob, meanMatrix: Matrix) =
        OpenCVFunctions.load_image(imgFile, blob.GetIntPtr(), meanMatrix.GetIntPtr())

    let CalculateMeanMatrix(data: float32[], chanels: int, meanWidth: int, meanHeight: int, imgWidth: int, imgHeight: int) =
        let dataHdl = GCHandle.Alloc(data, GCHandleType.Pinned)
        let matrix = OpenCVFunctions.calculate_mean_matrix(dataHdl.AddrOfPinnedObject(), chanels, meanWidth, meanHeight, imgWidth, imgHeight)
        dataHdl.Free()
        new Matrix(matrix)