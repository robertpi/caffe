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

EXPORT void load_image(char *file, Blob<float>* input_layer, cv::Mat *mean)
{
	cv::Mat img = cv::imread(file, -1);

	std::vector<cv::Mat> input_channels;

	int width = input_layer->width();
	int height = input_layer->height();
	float* input_data = input_layer->mutable_cpu_data();
	for (int i = 0; i < input_layer->channels(); ++i) {
		cv::Mat channel(height, width, CV_32FC1, input_data);
		input_channels.push_back(channel);
		input_data += width * height;
	}

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