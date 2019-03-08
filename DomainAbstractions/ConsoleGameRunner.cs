using GameScoring.ProgrammingParadigms;
using System;





namespace GameScoring.Application
{
    /// <summary>
    /// Console UI for running games. 
    /// Will prompt for input, pass the input to the scoringEngine (which is IConsistsOf), and then ask for an ASCII representation of the score to display. Repeats until the game completes.
    /// </summary>
    /// <remarks>
    /// It uses the IConsistsOf interface to talk to the scoringEngine to update the score, and know if it's complete.
    /// It uses a IPullDataFlow interface to get a string representation of the score.
    /// </remarks>
    public class ConsoleGameRunner
    {
        private IConsistsOf scorerEngine;
        private IPullDataFlow<string> scorecard;
        private readonly string prompt;
        private readonly Action<int, IConsistsOf> PlayLambda;




        /// <summary>
        /// Console UI for running games. 
        /// Will prompt for input, pass the input to the scoringEngine (which is IConsistsOf), and then ask for an ASCII representation of the score to display. Repeats until the game completes.
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
