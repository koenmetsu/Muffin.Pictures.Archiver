(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

(**
Running the application
-----------------------

Build the application with ``build.cmd`` (Windows) or ``build.sh`` (Mono).

#### Setting a source and destination directory

```text
    Muffin.Pictures.Archiver.exe --sourcedir c:\temp --destinationdir c:\temp\destination --fallback --mailto foo@dog.com
```

#### Example output

Running the application will output a report of which files were skipped, moved or failed to move.

Optionally, you can [configure Archiver to mail this report](configuration.html).

```console
Skipped files:
4 files were skipped.
Reason: File 123.png has no time taken.
Reason: File 2015-02-04_0901.png has no time taken.
Reason: File 2015-02-04_0901.png was not old enough to be moved.
Reason: File 789.png has no time taken.

Successes:
9 files were successfully moved.
        c:\demo\IMG_20150310_121043.jpg -> c:\destination\2015-03\IMG_20150310_121043.jpg
        c:\demo\IMG_20150317_150042.jpg -> c:\destination\2015-03\IMG_20150317_150042.jpg
        c:\demo\IMG_20150613_154925.jpg -> c:\destination\2002-12\IMG_20150613_154925.jpg
        c:\demo\IMG_20150613_154930.jpg -> c:\destination\2002-12\IMG_20150613_154930.jpg
        c:\demo\IMG_20150613_154932.jpg -> c:\destination\2002-12\IMG_20150613_154932.jpg
        c:\demo\IMG_20150613_154935.jpg -> c:\destination\2002-12\IMG_20150613_154935.jpg
        c:\demo\IMG_20150613_154936.jpg -> c:\destination\2002-12\IMG_20150613_154936.jpg
        c:\demo\IMG_20150613_154937.jpg -> c:\destination\2002-12\IMG_20150613_154937.jpg
        c:\demo\IMG_20150613_154938.jpg -> c:\destination\2002-12\IMG_20150613_154938.jpg

Failures:
0 files could not be moved.
Done archiving!
```

*)
