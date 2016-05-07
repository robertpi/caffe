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

EXPORT void *caffe_net_new(char *netFile, const Phase phase)
{
	return new Net<float>(netFile, phase);
}

EXPORT void *caffe_net_layer_by_name(void *netAnon, const char *layer_name)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->layer_by_name(layer_name).get();
}

EXPORT void *caffe_net_input_blob(void *netAnon, int i)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->input_blobs()[i];
}

EXPORT void caffe_net_Forward(void *netAnon, float &loss)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->Forward(&loss);
}

EXPORT float caffe_net_ForwardFromTo(void *netAnon, int start, int end)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->ForwardFromTo(start, end);
}

EXPORT float caffe_net_ForwardFrom(void *netAnon, int start)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->ForwardFrom(start);
}

EXPORT float caffe_net_ForwardTo(void *netAnon, int end)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->ForwardTo(end);
}

EXPORT void caffe_net_ClearParamDiffs(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->ClearParamDiffs();
}

EXPORT void caffe_net_Backward(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->Backward();
}

EXPORT void caffe_net_BackwardFromTo(void *netAnon, int start, int end)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->BackwardFromTo(start, end);
}

EXPORT void caffe_net_BackwardFrom(void *netAnon, int start)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->BackwardFrom(start);
}

EXPORT void caffe_net_BackwardTo(void *netAnon, int end)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->BackwardTo(end);
}

EXPORT void caffe_net_Reshape(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->Reshape();
}


EXPORT float caffe_net_ForwardBackward(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->ForwardBackward();
}

EXPORT void caffe_net_Update(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->Update();
}

EXPORT void caffe_net_ShareWeights(void *netAnon)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->ShareWeights();
}

EXPORT void caffe_net_CopyTrainedLayersFrom(void *netAnon, char *trained_filename)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->CopyTrainedLayersFrom(trained_filename);
}

EXPORT void caffe_net_CopyTrainedLayersFromBinaryProto(void *netAnon, char *trained_filename)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->CopyTrainedLayersFromBinaryProto(trained_filename);
}

EXPORT void caffe_net_CopyTrainedLayersFromHDF5(void *netAnon, char *trained_filename)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->CopyTrainedLayersFromHDF5(trained_filename);
}


EXPORT void caffe_net_ToHDF5(void *netAnon, char *filename, bool write_diff)
{
	Net<float> *net = (Net<float> *)netAnon;
	net->ToHDF5(filename, write_diff);
}


EXPORT const char *caffe_net_name(void *netAnon)
{ 
	Net<float> *net = (Net<float> *)netAnon;
	return net->name().c_str();
}

EXPORT const char *caffe_net_layer_name(void *netAnon, int i)
{ 
	Net<float> *net = (Net<float> *)netAnon;
	return net->layer_names()[i].c_str();
}

EXPORT const char *caffe_net_blob_name(void *netAnon, int i)
{ 
	Net<float> *net = (Net<float> *)netAnon;
	return net->blob_names()[i].c_str();
}

EXPORT void *caffe_net_blob(void *netAnon, int i)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->blobs()[i].get();
}

EXPORT void *caffe_net_layer(void *netAnon, int i)
{
	Net<float> *net = (Net<float> *)netAnon;
	return net->layers()[i].get();
}

EXPORT int caffe_net_phase(void *netAnon)
{ 
	Net<float> *net = (Net<float> *)netAnon;
	return net->phase();
}
