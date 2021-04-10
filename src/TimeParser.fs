module TimeParser
    open System
    open FSharp.Text.RegexProvider

    type ShortTerm = Regex<"""(?<week>W\d+)?(?<day>D\d+)?(?<hour>H\d+)?(?<minute>M\d+)?(?<second>S\d+)?""">

    let getTime (grp:System.Text.RegularExpressions.Group) =
        
        if grp.Success then
            let str = grp.Value
            let span = str.Substring(0, 1)
            let value = str.Substring(1) |> float

            let m =
                match span with
                | "W" -> (fun x -> TimeSpan.FromDays(x * 7.0))
                | "D" -> TimeSpan.FromDays
                | "H" -> TimeSpan.FromHours
                | "M" -> TimeSpan.FromMinutes
                | "S" -> TimeSpan.FromSeconds
                | _ -> (fun _ -> TimeSpan.Zero)

            m value
        else
            TimeSpan.Zero

    let parse (time: string) =
        match Double.TryParse time with
        | true, f -> TimeSpan.FromSeconds f
        | _ ->
            match ShortTerm().TryTypedMatch(time) with
            | Some m ->
                seq {
                    getTime m.week
                    getTime m.day
                    getTime m.hour
                    getTime m.minute
                    getTime m.second
                } |> Seq.fold (+) TimeSpan.Zero

            | None -> TimeSpan.Zero
        