namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr
open Caffe.Clr.Interop

type Scalar(scalarAnon: IntPtr) =
     member internal x.GetIntPtr() = scalarAnon

type Matrix(matrixAnon: IntPtr) =
     member internal x.GetIntPtr() = matrixAnon

     member x.Width 
        with get() = OpenCVFunctions.opencv_matrix_width(matrixAnon)

     member x.Height 
        with get() = OpenCVFunctions.opencv_matrix_height(matrixAnon)

     member x.ConvertTo(rtype: int) =
        let ptr = OpenCVFunctions.opencv_matrix_convertTo(matrixAnon, rtype)
        new Matrix(ptr)

     member x.Type() =
        OpenCVFunctions.opencv_matrix_type(matrixAnon)

     static member FromDimensions(width: int, height: int, t: int, initValue: Scalar) =
        let ptr = OpenCVFunctions.opencv_matrix_new(width, height, t, initValue.GetIntPtr())
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

    let MergeFloatArray(data: float32[], chanels: int, width: int, height: int) =
        let dataHdl = GCHandle.Alloc(data, GCHandleType.Pinned)
        let matrix = OpenCVFunctions.opencv_merge_float_array(dataHdl.AddrOfPinnedObject(), chanels, width, height)
        dataHdl.Free()
        new Matrix(matrix)

    let Mean(m: Matrix) =
        let ptr = OpenCVFunctions.opencv_mean(m.GetIntPtr())
        new Scalar(ptr)
