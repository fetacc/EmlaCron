module EmlaCron.EmlaClient

    open System
    open SwaggerProvider

    type Emlalock = SwaggerClientProvider<"swagger.yml">

    let client = Emlalock.Client()

    let runTask a = a |> Async.AwaitTask |> Async.RunSynchronously

    let getInfo userId apiKey =
        let info = client.GetInfo(apiKey, userId) |> runTask
        info

    let isUserInSession userId apiKey =
        let info = getInfo userId apiKey
        not (isNull info.Chastitysession.Chastitysessionid)

    let addReq userId apiKey req =
        client.GetAddrequirement(apiKey, userId, (string req)) |> runTask

    let addTime userId apiKey (time: TimeSpan) =
        let t = time.TotalSeconds |> Math.Ceiling |> string
        client.GetAdd(apiKey, userId, t) |> runTask
