### 1.5.3
* Use Serilog to do initial startup log 

### 1.5.2
* Remove test typo  

### 1.5.1
* Disable Serilog ExceptionDetails Enricher because it crashes on Mono 

### 1.5.0
* Print version number before starting a run 

### 1.4.0
* Log fatal errors to email 

### 1.2.0
* Remove NLog, log to ES instead

#### 1.1.0
* Report gps locations to ES 

#### 1.0.1
* Only process jpgs for now, vids are too much for comparison 

#### 0.4.1
* Use octopus tokens for nlog config as well 

#### 0.4.0
* Find time taken in file names as well
* Only send mails for failures

#### 0.3.0
* Change the way ES logs the move results 

#### 0.2.1
* Fix nlog config 

#### 0.2.0
* Add elastic reporter

#### 0.1.11
* Clean up install-exif 

#### 0.1.10
* Include NLog.config in bin 

#### 0.1.9
* Use bash to start install-exif postdeploy

#### 0.1.8
* Add octopusdeploy variable placeholders to smtp conf 

#### 0.1.7
* Fix build for mono 

#### 0.1.7.beta
* Install exiftool on postInstall 

#### 0.1.6.beta
* Reverse slashes in nuspec 

#### 0.1.5-beta
* Include exiftool in package

#### 0.1.4-beta
* Chmod to executable

#### 0.1.3-beta
* Add postDeploy script to chmod Muffin.Pictures.Archiver

#### 0.1.2-beta - May 29 2015
* trying out package without tools folder

#### 0.1.1-beta - May 29 2015
* Add missing exiftool exe

#### 0.1.0-beta - May 27 2015
* Files are moved to folders based on EXIF or XMP Date Taken
* Removed listing of files to archive


#### 0.0.1-beta - May 27 2015
* Initial release
* Added listing of files to archive
