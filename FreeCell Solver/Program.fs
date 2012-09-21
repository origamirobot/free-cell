// Learn more about F# at http://fsharp.net
open System
open FreeCell.Core

let main =
    Console.WriteLine("\nFREECELL SOLVER ")
    Console.WriteLine("--------------- ")
    let game = new Game(new DefaultRules())
    game.StartGame(1947380511)
    Console.WriteLine("Analyzing Game #: {0}\n\n", game.GameNumber)
    
    //while not game.Finished do
    //    Console.Write("Not Done Yet")
    
    

        

    Console.Read();


main

