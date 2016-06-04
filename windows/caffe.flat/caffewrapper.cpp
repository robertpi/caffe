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

EXPORT void caffe_set_mode(Caffe::Brew mode)
{
	Caffe::set_mode(mode);
}


EXPORT void caffe_SetDevice(int deviceId)
{
	Caffe::SetDevice(deviceId);
}

EXPORT void caffe_FindDevice()
{
	Caffe::FindDevice();
}

EXPORT void *caffe_ReadProtoFromBinaryFileOrDie(const char* filename)
{
	BlobProto proto;
	caffe::ReadProtoFromBinaryFileOrDie(filename, &proto);
	return &proto;
}