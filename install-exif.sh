#!/bin/bash
mkdir tmp
pushd tmp
wget http://www.sno.phy.queensu.ca/~phil/exiftool/Image-ExifTool-10.02.tar.gz
gzip -dc Image-ExifTool-10.02.tar.gz | tar -xf -
mv Image-ExifTool-10.02/exiftool ../lib/exiftool
mv Image-ExifTool-10.02/lib ../lib/
pushd Image-ExifTool-10.02
# perl Makefile.PL PREFIX=~/usr/bin LIB=~/usr/lib/exiftool/
# make test
# make install
popd
#cp exiftool ../lib/exiftool -r
# cp lib ../lib -r
popd
# rm tmp -rf
