#include "stdafx.h"

#pragma warning(push, 0) // disable warnings from the following external headers
#include <vector>
#include <string>
#include <stdio.h>
#include "caffe/caffe.hpp"
#include "caffe/blob.hpp"
#include "caffe/layers/input_layer.hpp"
#include "caffe/layers/memory_data_layer.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#pragma warning(push, 0) 

using namespace boost;
using namespace caffe;

EXPORT void caffe_SetDevice(int deviceId)
{
	// Set GPU
	if (deviceId >= 0)
	{
		Caffe::set_mode(Caffe::GPU);
		Caffe::SetDevice(deviceId);
	}
	else
	{
		Caffe::set_mode(Caffe::CPU);
	}
}
