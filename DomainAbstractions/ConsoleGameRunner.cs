using GameScoring.ProgrammingParadigms;
using System;





namespace GameScoring.Application
{
    /// <summary>
    /// Console UI for running games. 
    /// Will prompt for input, pass the input to the game it is wired to, and then ask the game for an ASCII representation of the score to display. Will repeat that until the game completes.
    /// </summary>
    /// <remarks>
    /// It uses the IGame interface to interface with a game
    /// </remarks>
    public class ConsoleGameRunner
    {
        private IConsistsOf scorerEngine;
        private IPullDataFlow<string> scorecard;
        private readonly string prompt;
        private readonly Action<int, IConsistsOf> PlayLambda;




        /// <summary>
        /// Console UI for running games. 
        /// It will prompt for input, pass the input to the game it is wired to, and then ask the game for an ASCII representation of the score to display. Will repeat that until the game completes.
        /// </summary>
        public ConsoleGameRunner(string prompt, Action<int, IConsistsOf> play) { this.prompt = prompt; PlayLambda = play; }




        public void Run()
        {
            // Console.Write(game.ToString());
            // Console.WriteLine();
            // Console.WriteLine(game.GetScore());
            while (!scorerEngine.IsComplete())
            {
                Console.WriteLine(prompt);
                int winner = Convert.ToInt32(Console.ReadLine());
                PlayLambda(winner, scorerEngine);
                // Console.Write(game.ToString());  // enable for debugging
                Console.WriteLine();
                Console.WriteLine(scorecard.GetData());
            }
            Console.WriteLine("GameOver");
            Console.ReadKey();
        }





    }
}
