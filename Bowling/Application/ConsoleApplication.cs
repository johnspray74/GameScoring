using System;




// This is just a console UI for the Bowling and Tennis scoreing applications.
// It uses BowlingApplication.cs or TennisApplication.cs to do all the real work of scoring

namespace Application
{
    class ConsoleApplication
    {
        static void Main(string[] args)
        {
            var game = new Tennis();
            // var game = new Bowling();

            // Console.Write(game.ToString());
            // Console.WriteLine();
            // Console.WriteLine(game.GetScore());
            while (!game.IsComplete())
            {
                Console.WriteLine("Enter score for bowling (0..9) or winner for tennis (0..1)");
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
