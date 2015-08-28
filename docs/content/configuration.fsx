(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Configuration
-------------

The Archiver can be configured with the following settings, either through command line parameters or through the ``Muffin.Pictures.Archiver.config``.
However, command line parameters have precedence over XML configuration.

*Through config:*


```xml
<appSettings>
    <add key="sourceDir" value="d:\temp\pictures-inbox"/>
    <add key="destinationDir" value="d:\pictures-final"/>
    <add key="fallback" value="true"/>
    <add key="mailto" value="foo@dog.com"/>
</appSettings>```

*Through the command line:*


```console
Muffin.Pictures.Archiver.exe --sourcedir d:\temp\pictures-inbox --destinationdir d:\pictures-final --fallback --mailto foo@dog.com
```

#### SourceDir
**Mandatory**

The directory from where to move the files.

#### DestinationDir
**Mandatory**

The directory where the files will be archived into folders by date.

#### Fallback
Set this flag if you want the Archiver to look at the `Date Modified` if it can't find the correct EXIF tag.

**NOTE: Enabling Fallback will also move any other file type, ie: not only pictures.**
#### MailTo
Set this if you want to send an email report of the archival process.

**NOTE: This will require you to configure smtp in the ``smtp.config`` file.**

*)
