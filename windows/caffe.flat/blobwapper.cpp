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

EXPORT void *caffe_blob_new(int *shape, int length)
{
	vector<int> shapeVec(shape, shape + length);
	return new Blob<float>(shapeVec);
}

EXPORT void caffe_blob_Reshape(void *blobAnon, int *shape, int length)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	vector<int> shapeVec(shape, shape + length);
	blob->Reshape(shapeVec);
}

EXPORT void caffe_blob_ReshapeLike(void *blobAnon, void *otherAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	Blob<float> &other = (Blob<float> &)otherAnon;
	blob->ReshapeLike(other);
}

EXPORT const char *caffe_blob_shape_string(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->shape_string().c_str();
}

// TODO another bloody vector
//  inline const vector<int>& shape() const { return shape_; }

EXPORT int caffe_blob_shape(void *blobAnon, int index)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->shape(index);
}

EXPORT int caffe_blob_num_axes(void *blobAnon)
{ 
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->num_axes();
}

EXPORT int caffe_blob_count(void *blobAnon) 
{ 
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->count();
}

EXPORT int caffe_blob_count_start_end(void *blobAnon, int start_axis, int end_axis)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->count(start_axis, end_axis);
}

EXPORT int caffe_blob_count_start(void *blobAnon, int start_axis)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->count(start_axis);
}

EXPORT int caffe_blob_CanonicalAxisIndex(void *blobAnon, int axis_index)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->CanonicalAxisIndex(axis_index);
}

EXPORT int caffe_blob_offset(void *blobAnon, int n, int c, int h, int w)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->offset(n, c, h, w);
}

EXPORT int caffe_blob_offset_vector(void *blobAnon, int *indices, int length)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	vector<int> indicesVec(indices, indices + length);
	return blob->offset(indicesVec);
}

EXPORT float caffe_blob_data_at(void *blobAnon, int n, int c, int h, int w)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->data_at(n, c, h, w);
}

EXPORT float caffe_blob_diff_at(void *blobAnon, int n, int c, int h, int w)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->diff_at(n, c, h, w);
}


EXPORT float caffe_blob_data_at_vector(void *blobAnon, int *index, int length)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	vector<int> indexVec(index, index + length);
	return blob->data_at(indexVec);
}

EXPORT float caffe_blob_diff_at_vector(void *blobAnon, int *index, int length)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	vector<int> indexVec(index, index + length);
	return blob->diff_at(indexVec);
}

EXPORT const float* caffe_blob_cpu_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->cpu_data();
}

EXPORT void caffe_blob_set_cpu_data(void *blobAnon, float* data)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	blob->set_cpu_data(data);
}

EXPORT const int* caffe_blob_gpu_shape(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->gpu_shape();
}

EXPORT const float* caffe_blob_gpu_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->gpu_data();
}

EXPORT const float* caffe_blob_cpu_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->cpu_diff();
}

EXPORT const float* caffe_blob_gpu_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->gpu_diff();
}

EXPORT float* caffe_blob_mutable_cpu_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->mutable_cpu_data();
}

EXPORT float* caffe_blob_mutable_gpu_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->mutable_gpu_data();
}

EXPORT float* caffe_blob_mutable_cpu_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->mutable_cpu_diff();
}

EXPORT float* caffe_blob_mutable_gpu_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->mutable_gpu_diff();
}

EXPORT void caffe_blob_Update(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	blob->Update();
}

EXPORT float caffe_blob_asum_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->asum_data();
}

EXPORT float caffe_blob_asum_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->asum_diff();
}

EXPORT float caffe_blob_sumsq_data(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->sumsq_data();
}

EXPORT float caffe_blob_sumsq_diff(void *blobAnon)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	return blob->sumsq_diff();
}

EXPORT void caffe_blob_scale_data(void *blobAnon, float scale_factor)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	blob->scale_data(scale_factor);
}

EXPORT void caffe_blob_scale_diff(void *blobAnon, float scale_factor)
{
	Blob<float> *blob = (Blob<float> *)blobAnon;
	blob->scale_diff(scale_factor);
}

