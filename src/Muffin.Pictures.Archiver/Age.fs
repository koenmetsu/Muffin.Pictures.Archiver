namespace Muffin.Pictures.Archiver

open System

module Age =
    let isOld (timeProvider : unit -> DateTimeOffset) {File=_; TakenOn=takenOn} =
        // todo: replace this with single time at start of the program,
        // ie: not a function but a DT value
        let currentTime = timeProvider ()
        currentTime.AddMonths(-1) > takenOn
