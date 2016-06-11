---
title: CaffeNet F# Deep Dream example
description: How to make deep dream image using the F# API.
category: example
include_in_docs: true
priority: 10
---

# Classifying ImageNet: using the F# API

This example demonstrates using the F# API to create "deep dream"
images. It is based on the example shown in [this python work 
book](https://github.com/google/deepdream/blob/master/dream.ipynb)

## Presentation

The code for a simple classifier written in F# code is shown in
`windows/examples/fs_deepdream/Program.fs`. The aim is
to produce the simplest classifier possible.

## Compiling

The example should be build as part of the windows build [described here](../../../README.md).

## Usage

To use the pre-trained CaffeNet model with the classification example,
you need to download it from the "Model Zoo" using the following
script:
```
./scripts/download_model_binary.py models/bvlc_reference_caffenet
```

Alteratively, the model can be downloaded here: http://dl.caffe.berkeleyvision.org/bvlc_reference_caffenet.caffemodel

(Models are > 100MB, so too large to be versioned by git)

The ImageNet mean file is also need to perform mean subtraction:
```
./data/ilsvrc12/get_ilsvrc_aux.sh
```

Alteratively, the data can be downloaded from: http://dl.caffe.berkeleyvision.org/caffe_ilsvrc12.tar.gz

Using the files that were downloaded, we product a dream form the provided cat
image (`examples/images/cat.jpg`) using this command:
```
.\windows\examples\fs_deepdream\bin\Debug\fs_deepdream.exe "models\bvlc_googlenet\deploy.prototxt" "models\bvlc_googlenet\bvlc_googlenet.caffemodel" "data\ilsvrc12\imagenet_mean.binaryproto" "examples\images\cat.jpg" "inception_4c/output"
```

Note, the cat is not a good example, as the process only really works on square images, future versions may address this.

The last parameter is the layer in the network that will used produce the dream. Not all layers in 
the can produce dreams, as not all layers have blobs associated with them, but many can.

Higher number layers reconize more detailed elements within the image:
![High layer deep dream](https://pbs.twimg.com/media/CjuMKUlWUAApaqZ.jpg:large)

Low number layers enhance textures and patterns the image:
![Low layer deep dream](https://pbs.twimg.com/media/CjxRPs8VEAAgptk.jpg:large)