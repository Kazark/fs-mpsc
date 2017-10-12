open System
open System.Collections.Concurrent

let blockingQueue = new BlockingCollection<string>(ConcurrentQueue<string>())

let consume() =
    while true do
        let msg = blockingQueue.Take()
        Console.ForegroundColor <- ConsoleColor.Cyan
        eprintfn "and %s: ..." msg
        Console.ForegroundColor <- ConsoleColor.White
    Console.ForegroundColor <- ConsoleColor.DarkCyan
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
        |> Array.iter blockingQueue.Add
    0
