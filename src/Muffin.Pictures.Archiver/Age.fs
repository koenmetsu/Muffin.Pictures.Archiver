namespace Muffin.Pictures.Archiver

open System
open Domain

module Age =
    let isOld (timeProvider : unit -> DateTimeOffset) {File=_; TakenOn=takenOn} =
        let time = timeProvider ()
        time.AddMonths(-1) > takenOn