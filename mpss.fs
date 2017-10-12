open System
open System.Collections.Concurrent

let queue = ConcurrentQueue<string>()

let consume() =
    Console.ForegroundColor <- ConsoleColor.DarkCyan
    Seq.iter (eprintfn "and %s: ...") queue
    eprintfn "That's all."
    Console.ForegroundColor <- ConsoleColor.White

[<EntryPoint>]
let main _ =
    async {
        consume()
    } |> Async.Start
    while true do
        Console.ForegroundColor <- ConsoleColor.DarkMagenta
        printf "What else can you tell me? "
        Console.ForegroundColor <- ConsoleColor.White
        Console.ReadLine().Split ' '
        |> Array.iter queue.Enqueue
    0
