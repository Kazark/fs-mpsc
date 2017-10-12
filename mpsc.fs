open System
open System.Threading
open System.Collections.Concurrent

let mutable finished = false
let blockingQueue = new BlockingCollection<string>(ConcurrentQueue<string>())

let consume cancel =
    try
        while not finished do
            let msg = blockingQueue.Take(cancel)
            Console.ForegroundColor <- ConsoleColor.Cyan
            eprintfn "and %s: ..." msg
            Console.ForegroundColor <- ConsoleColor.White
            Thread.Sleep(msg.Length * 10)
    with _ ->
        Console.ForegroundColor <- ConsoleColor.DarkCyan
        eprintfn "That's all."
        Console.ForegroundColor <- ConsoleColor.White

let readLine() =
    try
        Console.ReadLine()
    with ex ->
        printfn "ReadLine exception: %O" ex
        null

[<EntryPoint>]
let main _ =
    Async.Start(async { consume(Async.DefaultCancellationToken) }, Async.DefaultCancellationToken)
    try
        let rand = Random()
        Seq.initInfinite (fun _ ->
            Thread.Sleep(rand.Next(1,100))
            Console.ForegroundColor <- ConsoleColor.DarkMagenta
            printf "What else can you tell me? "
            Console.ForegroundColor <- ConsoleColor.White
            readLine()
        ) |> Seq.takeWhile ((<>) null)
        |> Seq.iter (fun (x : string) ->
            x.Split ' '
            |> Array.filter (not << String.IsNullOrWhiteSpace)
            |> Array.iter blockingQueue.Add
        )
    with ex ->
        printfn "Exception: %O" ex
    Async.CancelDefaultToken()
    0
