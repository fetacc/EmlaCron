// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open CommandLine
open EmlaCron.EmlaClient

type options = {
  [<Option('u', "userId", Required = true, HelpText = "User Id")>] userId : string
  [<Option('a', "apiKey", Required = true, HelpText = "API Key")>] apiKey : string
  [<Option('r', "requirements", Required = false, HelpText = "Requirement Threshold - If specified, when remaining requirements drop below this amount the amount will be added again. Will not be added if no remaining requirements")>] requirements : int64 option
  [<Option('t', "time", Required = false, HelpText = "Time Threshold - If specified, this time will be added to the duration if there is less time remaining than this until release. Will not be added if time remaining is zero")>] time : string option
  [<Option('f', "force", Required = false, HelpText = "Force - Will add requirements and time even if remaining time or requirements are at zero")>] force : bool
}

let runRequirements options (info : Emlalock.Model) =
    match options.requirements with
    | Some x ->
        printfn "The req threshold is %i, currently there are %i" x info.Chastitysession.Requirements.Value

        let willAddRequirements =
            info.Chastitysession.Requirements.Value < x // Less than x requirements remain
            && (options.force || info.Chastitysession.Requirements.Value > 0L) // Remaning requirements greater than 0 or Force

        if willAddRequirements then
            addReq options.userId options.apiKey x |> ignore
            printfn "Add %i requirements" x
    | None -> printfn "No requirements threshold"

let runTime options (info : Emlalock.Model) =
    match options.time with
    | Some x ->
        let t = TimeParser.parse x
        printfn "The time threshold is %A" t

        let willAddTime =
            let projectedRelease = (info.Chastitysession.Startdate.Value + info.Chastitysession.Duration.Value) |> DateTimeOffset.FromUnixTimeSeconds
            
            (DateTime.UtcNow.Add t) > projectedRelease.DateTime // Projected release date less than t in the future
            && (options.force || DateTime.UtcNow < projectedRelease.DateTime) // Force or release date is still in future

        
        if willAddTime then
            addTime options.userId options.apiKey t |> ignore
            printfn "Added %A to duration" t

    | None -> printfn "No time threshold"


let run parsedValues =
    let userId = parsedValues.userId
    let apiKey = parsedValues.apiKey

    let info = getInfo userId apiKey
    
    let isInSession = isUserInSession userId apiKey

    if isInSession then
        runRequirements parsedValues info
        runTime parsedValues info
    else
        printfn "User %s not in an active session, exiting now" info.User.Username

    0

[<EntryPoint>]
let main argv =

  
    let result = CommandLine.Parser.Default.ParseArguments<options>(argv)
    match result with
    | :? Parsed<options> as parsed -> run parsed.Value
    | _ -> 1
