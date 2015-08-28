(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Configuration
-------------

The Archiver can be configured with the following settings, either through command line parameters or through the ``Muffin.Pictures.Archiver.config``. 
However, command line parameters have precedence over XML configuration.

- SourceDir
  - (**Mandatory**) The directory from where to move the files.
- DestinationDir
  - (**Mandatory**) The directory where the files will be archived into folders by date.
- Fallback
  - Set this flag if you want the Archiver to look at the date modified if it can't find the correct EXIF tag.
  - This will also consequently move any other file type.
- MailTo
  - Set this if you want to send an email report of the archival process.
  - This will require you to configure smtp in the ``smtp.config`` file. 
*)