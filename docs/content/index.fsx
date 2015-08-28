(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Muffin.Pictures.Archiver
======================

> Archiver - *pet project that archives pictures based on their date taken.*
>
> Pictures - *what it's supposed to be used for*.<br />
>
> Muffin - *the name of our NAS (wife got to choose the name).*

Archiver takes a folder with pictures, and tries to organize them by date taken.

Archiver tries to find out when a pictures was taken based on the picture's ``XMP-xmp:CreateDate`` or ``ExifIFD:CreateDate`` tag, and moves them to folders based on the year-month.

If Archiver cannot find the time taken through EXIF tags, it can optionally fall back to the ``Date Modified`` of the file.

Getting started
-------
See how to [build and run the application](running.html).

See how to [configure the application](configuration.html).

Example
-------

Running Archiver with default settings, the following files at ``sourcedir``
*)

| File  | DateTaken        | Date Modified    |
| ----- | ---------------- | ---------------- |
| File1 | 2015-02-03 17:23 | 2014-02-03 17:23 |
| File2 |                  | 2014-07-03 17:23 |
| File3 | 2014-07-14 08:25 | 2015-01-20 22:59 |

(**
will result in the following structure at ``destinationDir``. ``File2`` is skipped because Archiver could not find a ``Date Taken``
*)

.
|-- 2015-02
    |-- File1
|-- 2014-07
    |-- File3

(**
Running the archiver with ``fallback`` mode enabled will also move ``File2``, based on it's ``Date Modified`` tag:
*)

.
|-- 2015-02
    |-- File1
|-- 2014-07
    |-- File2
    |-- File3

(**
Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork
the project and submit pull requests. If you're adding a new public API, please also
consider adding [samples][content] that can be turned into a documentation. You might
also want to read the [library design notes][readme] to understand how it works.

The library is available under Public Domain license, which allows modification and
redistribution for both commercial and non-commercial purposes. For more information see the
[License file][license] in the GitHub repository.

  [content]: https://github.com/koenmetsu/Muffin.Pictures.Archiver/tree/master/docs/content
  [gh]: https://github.com/koenmetsu/Muffin.Pictures.Archiver
  [issues]: https://github.com/koenmetsu/Muffin.Pictures.Archiver/issues
  [readme]: https://github.com/koenmetsu/Muffin.Pictures.Archiver/blob/master/README.md
  [license]: https://github.com/koenmetsu/Muffin.Pictures.Archiver/blob/master/LICENSE.txt
*)
