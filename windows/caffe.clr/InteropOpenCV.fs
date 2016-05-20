namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices
open Caffe.Clr

module OpenCVFunctions =
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr opencv_imread(string file, int i)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void opencv_split_to_input_blob(IntPtr img, IntPtr input_blob)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr opencv_matrix_convertTo(IntPtr target, int rtype)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int opencv_matrix_height(IntPtr target)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern int opencv_matrix_width(IntPtr target)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr opencv_resize(IntPtr m, int width, int height)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr opencv_subtract(IntPtr mX, IntPtr mY)

    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void load_image(string imgFile, IntPtr inputBlob, IntPtr meanMatrix);
    
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr calculate_mean_matrix(IntPtr data, int num_channels, int mean_width, int mean_height, int img_width, int img_height)

