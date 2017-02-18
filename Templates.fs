namespace BjoernerlweinDe.Core

open BjoernerlweinDe.Core.ListML.Core
open BjoernerlweinDe.Core.ListML.HTML
open BjoernerlweinDe.Core.Settings

module Templates =

    let scripts =
        match CurrentEnvironment with
        | Production -> ["/js/script.min.js"]
        | Development -> ["/bower_components/nanoajax/nanoajax.min.js"; "/bower_components/vue/dist/vue.js";"/js/script.js"]
        |> List.fold (fun acc elem ->
            acc + script ["src", elem] [""]
        ) ""

    let stylesheets = 
        match CurrentEnvironment with
        | Production -> ["/css/style.min.css"]
        | Development -> ["/bower_components/normalize-css/normalize.css"; "/css/style.css"]
        |> List.fold (fun acc elem ->
            acc + link ["href", elem; "rel", "stylesheet"] [] ) ""

    let vueDebugMode =
        match CurrentEnvironment with
        | Production -> ""
        | Development ->
            script ["type", "text/javascript"] ["Vue.config.debug = true;"]

    let page =
        doctype +
        html ["lang","de"] [
            head [] [
                meta ["charset", "UTF-8"]
                meta ["http-equiv", "X-UA-Compatible"; "content", "IE=edge"]
                meta ["name", "viewport"; "content", "width=device-width,initial-scale=1"]
                title ["id", "title"] ["Bjoernerlwein.de"]
                stylesheets
                link [href "//fonts.googleapis.com/css?family=Source+Sans+Pro:400,700,400italic,700italic";  "rel", "stylesheet";  "type", "text/css"] []
                
                script ["type", "x-template"; "id", "staticpagelist-tpl"] [
                    ul [] [
                        li ["v-for" ,"page in staticpages"] [
                            a [":href", "'/#/staticpage/' + page.id"] ["{{page.title}}"]]]]
                script ["type", "x-template"; "id", "post-tpl"] [
                    article [] [
                        h1 [] ["{{title}}"]
                        small [] ["{{date}}"]
                        div ["v-html", "content"] []]]              
                script ["type", "x-template"; "id", "posts-index-tpl"] [
                    div [] [
                        article ["v-for", "post in posts"] [
                            h1 [] [
                                a [":href", "'#/post/' + post.id"] ["{{post.title}}"]]
                            small [] ["{{post.date}}"]
                            div ["v-html", "post.content"] []]]]
                script ["type", "x-template"; "id", "ti-tpl"] [
                    div [] [
                        div ["class", "ti-anleitung"] [
                            "In die Felder die Spieler in Würfelreihenfolge eingeben und dann den Knopf drücken"]
                        table ["class", "ti-race-pick"] [
                            tbody [] [
                                tr ["v-for", "slot in slots"] [
                                    td [] [
                                        input ["type", "text"; "class", "ti-player"] []]]]
                            tfoot [] [
                                td [] [
                                    button ["type", "button"; "v-on:click", "submit()"] ["Dann mach mal"]]]]
                        ol ["class", "result-list"] [
                            li ["v-for", "result in results";] [
                                strong [] ["{{result.name}}"]
                                " spielt heute: "
                                strong [] ["{{result.race}}"]]]]]]
            body [] [
                div [classAttr "sidebar"] [
                    header [] [
                        h1 [] [
                            a [href "/#"] ["Bjoernerlwein.de"]]
                        small [classAttr "sub-header"; "id", "pgtitle"] [""]]
                    nav [classAttr "navigation"] [
                        h2 [] ["Pages"]
                        div ["id", "staticpageslist"] [""]]
                    footer [] [
                        a [href "//creativecommons.org/licenses/by-sa/4.0/";  "rel", "license"] [
                            img [src "//i.creativecommons.org/l/by-sa/4.0/88x31.png"; alt "Creative Commons License";  "style", "border-width: 0;"]]
                        br []
                        span [] ["bjoernerlwein.de"]
                        " by "
                        a [href "//bjoernerlwein.de";  "rel", "cc:attributionURL"] ["Björn Erlwein"]
                        " is licensed under a "
                        a [href "//creativecommons.org/licenses/by-sa/4.0/";  "rel", "license"] ["Creative Commons Attribution-ShareAlike 4.0
                        International License"] 
                        "."]]
                div [classAttr "content"; "id", "app"] [
                    node "component" [":is", "currentView"] [""]]
                scripts
                vueDebugMode]]                