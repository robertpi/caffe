---
title: CaffeNet F# Classification example
description: A simple example performing image classification using the F# API.
category: example
include_in_docs: true
priority: 10
---

# Classifying ImageNet: using the F# API

This example demonstrates using the F# API to implement a simple
image classifer. It is a port of the C++ example.

## Presentation

A simple F# code is proposed in
`windows/examples/fs_classification/classification.cpp`. For the sake of
simplicity, this example does not support oversampling of a single
sample nor batching of multiple independant samples. This example is
not trying to reach the maximum possible classification throughput on
a system, but special care was given to avoid unnecessary
pessimization while keeping the code readable.

## Compiling

The example should be build as part of the windows build [described here](../../../blob/master/README.md).

## Usage

To use the pre-trained CaffeNet model with the classification example,
you need to download it from the "Model Zoo" using the following
script:
```
./scripts/download_model_binary.py models/bvlc_reference_caffenet
```

Alteratively, the model can be downloaded here: http://dl.caffe.berkeleyvision.org/bvlc_reference_caffenet.caffemodel

(Models are < 100MB, so too large to be versioned by git)

The ImageNet labels file (also called the *synset file*) is also
required in order to map a prediction to the name of the class:
```
./data/ilsvrc12/get_ilsvrc_aux.sh
```

Alteratively, the data can be downloaded from: http://dl.caffe.berkeleyvision.org/caffe_ilsvrc12.tar.gz

Using the files that were downloaded, we can classify the provided cat
image (`examples/images/cat.jpg`) using this command:
```
.\windows\examples\fs_classification\bin\Debug\fs_classification.exe "models\bvlc_reference_caffenet\deploy.prototxt" "models\bvlc_reference_caffenet\bvlc_reference_caffenet.caffemodel" "data\ilsvrc12\imagenet_mean.binaryproto" "data\ilsvrc12\synset_words.txt" "examples\images\cat.jpg"
```
The output should look like this:
```
0.317874 n02123045 tabby, tabby cat
0.240782 n02123159 tiger cat
0.126425 n02124075 Egyptian cat
0.110704 n02119022 red fox, Vulpes vulpes
0.067777 n02119789 kit fox, Vulpes macrotis
0.061915 n02127052 lynx, catamount
0.010351 n02123394 Persian cat
0.009120 n04493381 tub, vat
0.005259 n02120505 grey fox, gray fox, Urocyon cinereoargenteus
0.005089 n02112018 Pomeranian
```

