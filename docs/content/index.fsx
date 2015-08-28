(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Muffin.Pictures.Archiver
======================

Muffin - *the name of our NAS (wife got to choose the name).* <br />
Pictures - *what the archiver can work with*.<br />
Archiver - *what this application does.*

Archiver tries to find out when a pictures were taken based on the EXIF tag present in jpgs, and moves them to folders based on the year-month.
If archiver cannot find the time taken through EXIF tags, it can optionally fall back on the modified date of the file.

Example
-------

Given a number of files
*)

| File  | DateTaken        | Date Modified    |
| ----- | ---------------- | ---------------- |
| File1 | 2015-02-03 17:23 | 2014-02-03 17:23 |
| File2 |                  | 2014-07-03 17:23 |
| File3 | 2014-07-14 08:25 | 2015-01-20 22:59 |

(**
the following output will be created by the Archiver (with fallback mode enabled):

*)

.
|-- 2015-02
|   |--- File1
|-- 2014-07
|   |--- File2
|   |--- File3

(**

Running the application
-----------------------

Build the application with ``build.cmd`` or ``build.sh``.

Run the Archiver eg in the command line:

```
Muffin.Pictures.Archiver.exe --sourcedir c:\temp --destinationdir c:\temp\destination --fallback --mailto abc@def.com
```

Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork
the project and submit pull requests. If you're adding a new public API, please also
consider adding [samples][content] that can be turned into a documentation. You might
also want to read the [library design notes][readme] to understand how it works.

The library is available under Public Domain license, which allows modification and
redistribution for both commercial and non-commercial purposes. For more information see the
[License file][license] in the GitHub repository.

  [content]: https://github.com/fsprojects/Muffin.Pictures.Archiver/tree/master/docs/content
  [gh]: https://github.com/fsprojects/Muffin.Pictures.Archiver
  [issues]: https://github.com/fsprojects/Muffin.Pictures.Archiver/issues
  [readme]: https://github.com/fsprojects/Muffin.Pictures.Archiver/blob/master/README.md
  [license]: https://github.com/fsprojects/Muffin.Pictures.Archiver/blob/master/LICENSE.txt
*)
