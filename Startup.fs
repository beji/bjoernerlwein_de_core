namespace BjoernerlweinDe.Core.App

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Debug
open Microsoft.Extensions.DependencyInjection
open Giraffe.HttpHandlers
open Giraffe.Tasks
open Giraffe.Middleware
open Microsoft.AspNetCore.StaticFiles
open Microsoft.AspNetCore.ResponseCompression
open BjoernerlweinDe.Core
open Newtonsoft.Json

module App =

    let handleTiRequest () =
        fun (next: HttpFunc) (ctx:  HttpContext) ->
            task {
                let form = ctx.Request.Form
                let success, players = form.TryGetValue("players")

                if success then
                    let races = TwilightImperium.getRacesForPlayers (string players) TwilightImperium.races
                    return! json races next ctx
                else
                    return! setStatusCode 404 next ctx
            }
        

    let webApp =
        choose [
            route "/" >=> setBodyAsString BjoernerlweinDe.Core.Templates.page
            route "/posts" >=> json (ParseMarkdown.getAllPosts)
            route "/staticpages" >=> json (ParseMarkdown.getStaticPagesWithoutContent)
            routef "/staticpage/%s" (fun id ->
                ParseMarkdown.getAllStaticPages
                |> List.find (fun item -> item.id = id)
                |> json
            )
            POST >=> route "/ti" >=> handleTiRequest() ]

    type Startup() =

        member __.ConfigureServices (services : IServiceCollection) =
            services.AddResponseCompression(fun options ->
                options.EnableForHttps <- true
                options.Providers.Add<GzipCompressionProvider>()) |> ignore

        member __.Configure  (app : IApplicationBuilder)
                            (env: IHostingEnvironment)
                            (loggerFactory: ILoggerFactory) =
            
            if env.IsDevelopment() then
                loggerFactory.AddConsole().AddDebug() |> ignore
            else
                loggerFactory.AddConsole() |> ignore
            app.UseResponseCompression().UseStaticFiles().UseGiraffe(webApp)
            