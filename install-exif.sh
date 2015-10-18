pushd lib
wget http://www.sno.phy.queensu.ca/~phil/exiftool/Image-ExifTool-10.02.tar.gz
gzip -dc Image-ExifTool-10.02.tar.gz | tar -xf -
pushd Image-ExifTool-10.02
perl Makefile.PL
make test
make install
cp exiftool ../exiftool
popd
rm Image-ExifTool-10.02* -rf
