namespace Muffin.Pictures.Archiver

module Paths =
    
    let pathCombine basePath target =
        System.IO.Path.Combine(basePath, target)
