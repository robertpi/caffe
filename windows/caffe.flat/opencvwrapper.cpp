#include "stdafx.h"

#pragma warning(push, 0) // disable warnings from the following external headers
#include <caffe/caffe.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <algorithm>
#include <iosfwd>
#include <memory>
#include <string>
#include <utility>
#include <vector>
#pragma warning(push, 0) 

using namespace caffe;  // NOLINT(build/namespaces)
using std::string;

EXPORT void *opencv_imread(char *file, int i)
{
	cv::Mat img = cv::imread(file, i);
	return new cv::Mat(img);
}

EXPORT void opencv_imwrite(char *file, cv::Mat* img)
{
	cv::imwrite(file, *img);
}


EXPORT void opencv_split_to_input_blob(cv::Mat *img, Blob<float>* input_blob)
{
	// need to read these here because of net_size, hopefully we'd fix that when all is torn apart
	int width = input_blob->width();
	int height = input_blob->height();

	// load from the blob into the input layer in a strange way
	std::vector<cv::Mat> input_channels;

	float* input_data = input_blob->mutable_cpu_data();
	for (int i = 0; i < input_blob->channels(); ++i) {
		cv::Mat channel(height, width, CV_32FC1, input_data);
		input_channels.push_back(channel);
		input_data += width * height;
	}

	cv::split(*img, input_channels);
}

EXPORT void *opencv_matrix_convertTo(cv::Mat* target, int rtype)
{
	cv::Mat *m = new cv::Mat();
	target->convertTo(*m, rtype);
	return m;
}

EXPORT int opencv_matrix_height(cv::Mat* target)
{
	return target->size().height;
}

EXPORT int opencv_matrix_width(cv::Mat* target)
{
	return target->size().width;
}

EXPORT int opencv_matrix_type(cv::Mat* target)
{
	return target->type();
}

EXPORT void *opencv_matrix_new_from_scalar(int rows, int cols, int type, cv::Scalar *init_value)
{
	return new cv::Mat(rows, cols, type, *init_value);
}

EXPORT void *opencv_matrix_new_from_data(int rows, int cols, int type, void *data)
{
	return new cv::Mat(rows, cols, type, data);
}

EXPORT void *opencv_resize(cv::Mat *m, int width, int height)
{
	cv::Mat *target = new cv::Mat();
	cv::Size target_size(width, height);
	cv::resize(*m, *target, target_size);
	return target;
}

EXPORT void *opencv_subtract(cv::Mat *mX, cv::Mat *mY)
{
	cv::Mat *mZ = new cv::Mat();
	cv::subtract(*mX, *mY, *mZ);
	return mZ;
}

EXPORT void *opencv_add(cv::Mat *mX, cv::Mat *mY)
{
	cv::Mat *mZ = new cv::Mat();
	cv::add(*mX, *mY, *mZ);
	return mZ;
}

EXPORT void *opencv_merge_float_array(float *data, int num_channels, int width, int height)
{
	std::vector<cv::Mat> channels;
	for (int i = 0; i < num_channels; ++i) {
		/* Extract an individual channel. */
		cv::Mat channel(height, width, CV_32FC1, data);
		channels.push_back(channel);
		data += height * width;
	}

	/* Merge the separate channels into a single image. */
	cv::Mat *mean =  new cv::Mat();
	cv::merge(channels, *mean);
	return mean;
}

EXPORT void *opencv_mean(cv::Mat *mean)
{
	cv::Scalar channel_mean = cv::mean(*mean);
	return new cv::Scalar(channel_mean);
}
