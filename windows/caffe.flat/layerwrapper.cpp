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

EXPORT bool caffe_layer_ShareInParallel(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->ShareInParallel();
}

EXPORT bool caffe_layer_IsShared(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->ShareInParallel();
}

EXPORT void caffe_layer_SetShared(void *layerAnon, bool is_shared)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->SetShared(is_shared);
}

EXPORT void caffe_layer_Reshape(void *layerAnon, void *bottomAnon, int bottomLength, void *topAnon, int topLength)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	Blob<float>** bottomArray = (Blob<float>**)bottomAnon;
	Blob<float>** topArray = (Blob<float>**)topAnon;
	vector<Blob<float>*> bottom(bottomArray, bottomArray + bottomLength);
	vector<Blob<float>*> top(topArray, topArray + topLength);
	return layer->Reshape(bottom, top);
}

EXPORT float caffe_layer_Forward(void *layerAnon, void *bottomAnon, int bottomLength, void *topAnon, int topLength)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	Blob<float>** bottomArray = (Blob<float>**)bottomAnon;
	Blob<float>** topArray = (Blob<float>**)topAnon;
	vector<Blob<float>*> bottom (bottomArray, bottomArray + bottomLength);
	vector<Blob<float>*> top (topArray, topArray + topLength);
	return layer->Forward(bottom, top);
}

EXPORT void caffe_layer_Backward(void *layerAnon, void *topAnon, void *propagate_downAnon, void *bottomAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	vector<Blob<float>*>& top = (vector<Blob<float>*>&)topAnon;
	vector<bool>& propagate_down = (vector<bool>&)propagate_downAnon;
	vector<Blob<float>*>& bottom = (vector<Blob<float>*>&)bottomAnon;
	layer->Backward(top, propagate_down, bottom);
}

EXPORT void *caffe_layer_blob(void *layerAnon, int i)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->blobs()[i].get();
}

EXPORT int caffe_layer_blobs_size(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->blobs().size();
}

EXPORT float caffe_layer_loss(void *layerAnon, int top_index)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->loss(top_index);
}

EXPORT void caffe_layer_set_loss(void *layerAnon, int top_index, float value)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->set_loss(top_index, value);
}

EXPORT const char* caffe_layer_type(void *layerAnon)
{ 
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->type();
}

EXPORT int caffe_layer_ExactNumBottomBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->ExactNumBottomBlobs();
}

EXPORT int caffe_layer_MinBottomBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->MinBottomBlobs();
}

EXPORT int caffe_layer_MaxBottomBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->MaxBottomBlobs();
}

EXPORT int caffe_layer_ExactNumTopBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->ExactNumTopBlobs();
}

EXPORT int caffe_layer_MinTopBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->MinTopBlobs();
}


EXPORT int caffe_layer_MaxTopBlobs (void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->MaxTopBlobs();
}

EXPORT bool caffe_layer_EqualNumBottomTopBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->EqualNumBottomTopBlobs();
}


EXPORT bool caffe_layer_AutoTopBlobs(void *layerAnon)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->AutoTopBlobs();
}


EXPORT bool caffe_layer_AllowForceBackward(void *layerAnon, int bottom_index)
{
	Layer<float> *layer = (Layer<float> *)layerAnon;
	return layer->AllowForceBackward(bottom_index);
}