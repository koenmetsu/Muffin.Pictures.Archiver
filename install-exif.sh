#!/bin/bash
mkdir tmp
pushd tmp
wget http://www.sno.phy.queensu.ca/~phil/exiftool/Image-ExifTool-10.02.tar.gz
gzip -dc Image-ExifTool-10.02.tar.gz | tar -xf -
pushd Image-ExifTool-10.02
perl Makefile.PL
make test
make PREFIX=. install
cp exiftool ../lib/exiftool
# cp lib ../lib -r
popd
rm tmp -rf
rm Image-ExifTool-10.02* -rf
