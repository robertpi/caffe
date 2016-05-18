namespace Caffe.Clr.Interop
open System
open System.Runtime.InteropServices
open Caffe.Clr

module OpenCVFunctions =
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern void load_image(string imgFile, IntPtr inputBlob, IntPtr meanMatrix);
    
    [<DllImport(Common.LibraryName, CharSet = CharSet.Ansi)>]
    extern IntPtr calculate_mean_matrix(IntPtr data, int num_channels, int mean_width, int mean_height, int img_width, int img_height)

