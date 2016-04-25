pushd lib
wget http://www.sno.phy.queensu.ca/~phil/exiftool/Image-ExifTool-10.02.tar.gz
gzip -dc Image-ExifTool-10.02.tar.gz | tar -xf -
cd Image-ExifTool-10.02
perl Makefile.PL
sudo make install
