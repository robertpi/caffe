namespace Caffe.Clr
open System
open System.Runtime.InteropServices
open Caffe.Clr
open Caffe.Clr.Interop

type Matrix(matrixAnon: IntPtr) =
     member internal x.GetIntPtr() = matrixAnon

module OpenCV =
    let LoadImage(imgFile: string, blob: Blob, meanMatrix: Matrix) =
        OpenCVFunctions.load_image(imgFile, blob.GetIntPtr(), meanMatrix.GetIntPtr())

    let CalculateMeanMatrix(data: float32[], chanels: int, meanWidth: int, meanHeight: int, imgWidth: int, imgHeight: int) =
        let dataHdl = GCHandle.Alloc(data, GCHandleType.Pinned)
        let matrix = OpenCVFunctions.calculate_mean_matrix(dataHdl.AddrOfPinnedObject(), chanels, meanWidth, meanHeight, imgWidth, imgHeight)
        dataHdl.Free()
        new Matrix(matrix)