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

EXPORT void load_image(char *file, Blob<float>* input_layer, cv::Mat *mean)
{
	// read image and do lots of resizing 
	cv::Mat img = cv::imread(file, -1);

	// need to read these here because of net_size, hopefully we'd fix that when all is torn apart
	int width = input_layer->width();
	int height = input_layer->height();

	cv::Size net_size(width, height);
	cv::Mat sample_resized;
	if (img.size() != net_size)
		cv::resize(img, sample_resized, net_size);
	else
		sample_resized = img;


	cv::Mat sample_float;
	sample_resized.convertTo(sample_float, CV_32FC3);

	cv::Mat sample_normalized;
	cv::subtract(sample_float, *mean, sample_normalized);

	// load from the blob into the input layer in a strange way
	std::vector<cv::Mat> input_channels;

	float* input_data = input_layer->mutable_cpu_data();
	for (int i = 0; i < input_layer->channels(); ++i) {
		cv::Mat channel(height, width, CV_32FC1, input_data);
		input_channels.push_back(channel);
		input_data += width * height;
	}

	cv::split(sample_normalized, input_channels);
}

EXPORT void *calculate_mean_matrix(float *data, int num_channels, int mean_width, int mean_height, int img_width, int img_height) {
	std::vector<cv::Mat> channels;
	for (int i = 0; i < num_channels; ++i) {
		/* Extract an individual channel. */
		cv::Mat channel(mean_height, mean_width, CV_32FC1, data);
		channels.push_back(channel);
		data += mean_height * mean_width;
	}

	/* Merge the separate channels into a single image. */
	cv::Mat mean;
	cv::merge(channels, mean);

	/* Compute the global mean pixel value and create a mean image
	* filled with this value. */
	cv::Scalar channel_mean = cv::mean(mean);
	return new cv::Mat(img_height, img_width, mean.type(), channel_mean);
}