namespace BjoernerlweinDe.Core

open System

module TwilightImperium =

    type Result = {name: string; race: string}

    let races =
        [|"Federation of Sol"; "Sardakk N'Orr"; "The Barony of Letnev"; "The Emirates of Hacan";
            "The L1Z1X Mindnet"; "The Mentak Coalition"; "The Naalu Collective"; "The Xxcha Kingdom"; "The Yssaril Tribes";
            "Universities of Jol-Nar"; "Clan of Saar"; "Embers of Muaat"; "The Winnu"; "The Yin Brotherhood"; "The Arborec"; "The Ghosts of Creuss"; "The Nekro Virus"|]
        |> Array.sort

    let KnuthShuffle (lst : array<'a>) =
        let rnd = new Random()
        let swap i j =                                                  // Standard swap
            let item = lst.[i]
            lst.[i] <- lst.[j]
            lst.[j] <- item

        let ln = lst.Length
        [0..(ln - 2)]                                                   // For all indices except the last
        |> Seq.iter (fun i -> swap i (rnd.Next(i, ln)))                 // swap th item at the index with a random one following it (or itself)
        lst                                                             // Return the list shuffled in place

    let prepareRequest (reqString:string) : string list =
        reqString.Split([|'|'|])
        |> Array.toList


    let testRequest = "Berlwein|Staumann|Krusator|Joe"

    let shuffleAndCutHead races =
        let tmp = KnuthShuffle races
        Array.head tmp, Array.tail tmp

    let getRacesForPlayers requestString races =

        let rec _internalLoop (names:string list) (races:string array) (acc: Result list) =
            match names with
            | [] -> acc
            | name::n_tail ->
                let race, rtail = shuffleAndCutHead races
                let pair = {name=name; race=race}
                _internalLoop n_tail rtail (pair :: acc)


        let names = prepareRequest requestString

        _internalLoop names races []
        |> List.rev    