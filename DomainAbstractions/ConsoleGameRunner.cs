using GameScoring.ProgrammingParadigms;
using System;





namespace GameScoring.Application
{
    /// <summary>
    /// Console UI for running games. 
    /// It will prompt for input, pass the input to the game it is wired to, and then ask the game for an ASCII representation of the score to display. Will repeat that until the game completes.
    /// </summary>
    /// <remarks>
    /// It uses the IGame interface to interface with a game
    /// </remarks>
    public class ConsoleGameRunner
    {
        private IGame game;  // wired by the WireTo method to a game class such as Tennis or Bowling
        private readonly string prompt;

        /// <summary>
        /// Console UI for running games. 
        /// It will prompt for input, pass the input to the game it is wired to, and then ask the game for an ASCII representation of the score to display. Will repeat that until the game completes.
        /// </summary>
        public ConsoleGameRunner(string prompt) { this.prompt = prompt; }

        public void Run()
        {
            // Console.Write(game.ToString());
            // Console.WriteLine();
            // Console.WriteLine(game.GetScore());
            while (!game.IsComplete())
            {
                Console.WriteLine(prompt);
                int winner = Convert.ToInt32(Console.ReadLine());
                game.Play(winner);
                // Console.Write(game.ToString());  // enable for debugging
                Console.WriteLine();
                Console.WriteLine(game.GetScore());
            }
            Console.WriteLine("GameOver");
            Console.ReadKey();
        }
    }
}
