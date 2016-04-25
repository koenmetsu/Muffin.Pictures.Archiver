#!/bin/bash
mkdir tmp
pushd tmp
wget http://www.sno.phy.queensu.ca/~phil/exiftool/Image-ExifTool-10.02.tar.gz
gzip -dc Image-ExifTool-10.02.tar.gz | tar -xf -
mv Image-ExifTool-10.02/exiftool ../exiftool
mv Image-ExifTool-10.02/lib/* ../
popd
rm -rf tmp
