// Learn more about F# at http://fsharp.org

open System
open System.IO
open Microsoft.AspNetCore.Hosting
open BjoernerlweinDe.Core
open BjoernerlweinDe.Core.App

[<EntryPoint>]
let main argv = 

    let host =
        WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<App.Startup>()
            .Build()
    host.Run()
    0
