namespace BjoernerlweinDe.Core

open System
open System.IO
open Markdig

module ParseMarkdown =

    type LineType = 
    | Text of string
    | Title of string
    | Date of string
    | Id of string

    let strip (stripChars:string) (text:string) =
        let idx = text.IndexOf(stripChars)
        match idx < 0 with
        | true -> text
        | false -> text.Remove(idx, stripChars.Length)

    let removeTitleLineHead = strip "::title "
    let removeDateLineHead = strip "::date "
    let removeIdLineHead = strip "::id "
    let lineHelper (line: string) =
    
        if line.StartsWith "::title" then
            removeTitleLineHead line
            |> Title 
        else if line.StartsWith "::date" then
            removeDateLineHead line
            |> Date
        else if line.StartsWith "::id" then
            removeIdLineHead line
            |> Id
        else 
            Text line

    type Content = {title : string; content : string; id: string; date : string}

    let getAllFiles path =
        let dir = DirectoryInfo(path)

        match dir.Exists with
        | true ->
            dir.GetFiles()
            |> Array.map (fun item ->
                sprintf "%s/%s" path item.Name
            )
            |> Array.toList
        | false -> []

    let postsPath = "./content/posts"
    let staticPath = "./content/pages"
        
    let parse lines =
        let rec _parse parsed lines =
            match lines with
            | [] -> parsed
            | head :: tail ->
                match lineHelper head with
                | Text line ->
                    _parse {parsed with content = (sprintf "%s\n%s" parsed.content line)} tail
                | Title line ->
                    _parse {parsed with title = line} tail
                | Date line -> 
                    _parse {parsed with date = line} tail
                | Id line ->
                    _parse {parsed with id = line} tail
        let init = {title = ""; content = ""; id = ""; date = ""}
        _parse init lines

    let parseMarkdown (content: Content) =
        {content with content = Markdown.ToHtml(content.content)}

    let parseAllFiles = 
        List.map ((File.ReadAllLines >> Array.toList) >> parse >> parseMarkdown)

    let sortByDate = 
        List.toSeq >> Seq.sortByDescending (fun (item:Content) -> System.DateTime.Parse item.date) >> Seq.toList        
    let getAllPosts =
        getAllFiles postsPath 
        |> parseAllFiles
        |> sortByDate
        
    let getAllStaticPages =
        getAllFiles staticPath |> parseAllFiles

    let getStaticPagesWithoutContent =
        getAllStaticPages
        |> List.map(fun content ->
            {content with content = ""}
        )