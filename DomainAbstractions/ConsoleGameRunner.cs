using GameScoring.ProgrammingParadigms;
using System;





namespace GameScoring.Application
{
    /// <summary>
    /// This is just a console UI for running games
    /// For example it can run the Bowling or Tennis scoring games.
    /// It uses the IGame interface to interface with a game
    /// </summary>
    public class ConsoleGameRunner
    {
        private IGame game;  // wired by the WireTo method to a game class such as Tennis or Bowling
        private readonly string prompt;

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
