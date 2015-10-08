# Muffin.Pictures.Archiver

> Archiver - *pet project that archives pictures based on their date taken.*
>
> Pictures - *what it's supposed to be used for*.<br />
>
> Muffin - *the name of our NAS, where I'll run this application (wife got to choose the name).*

Personal learning project for:
- F#
- ProjectScaffold
- OctopusDeploy
- Mono
 
[![Build status](https://ci.appveyor.com/api/projects/status/y5qda1k0udpaen68?svg=true)](https://ci.appveyor.com/project/koenmetsu/muffin-pictures-archiver)

---

Archiver takes a folder with pictures, and tries to organize them by date taken.

Archiver tries to find out when a pictures was taken based on the picture's ``XMP-xmp:CreateDate`` or ``ExifIFD:DateTimeOriginal`` tag, and moves them to folders based on the year-month.

If Archiver cannot find the time taken through EXIF tags, it can optionally fall back to the ``Date Modified`` of the file.

See [the website](https://koenmetsu.github.io/Muffin.Pictures.Archiver) for examples and more information on getting started and examples.
