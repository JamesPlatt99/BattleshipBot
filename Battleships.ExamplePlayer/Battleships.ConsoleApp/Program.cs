using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Player.Interface;

namespace Battleships.ConsoleApp
{
    class Program
    {
        private enum Winner
        {
            Player,
            Computer
        }
        static void Main(string[] args)
        {
            Console.WriteLine("This console app will test your bot against a fairly capable bot - be warned, it will attempt to copy you!");
            Console.WriteLine("To see how your bot behaves, you can add breakpoints to see what is happening throughout the games");
            Console.WriteLine("By running multiple rounds, you can see how you bot will behave in a league");
            Console.WriteLine("How many rounds would you like to run?");
            while (true)
            {
                var numberOfRounds = Console.ReadLine();
                if (!IsValid(numberOfRounds))
                {
                    Console.WriteLine("You must enter a integer between 1 and 101:");
                    continue;
                }
                RunMatch(Convert.ToInt32(numberOfRounds));
                break;
            }
            Console.ReadKey();
        }

        private static void RunMatch(int numberOfRounds)
        {
            Console.WriteLine("Running Match with {0} rounds", numberOfRounds);
            var playerBot = new ExamplePlayer.ExamplePlayer();
            var computerBot = new CopyBot.CopyBot();
            var playerOneFirst = true;
            var gameResults = new List<Winner>();

            for (var i = 0; i < numberOfRounds; i++)
            {
                gameResults.Add(playerOneFirst ? FindWinner(playerBot, computerBot) : FindWinner(computerBot, playerBot));
                playerOneFirst = !playerOneFirst;
            }

            Console.WriteLine("{0} was played against {1} for {2} rounds", playerBot.Name, computerBot.Name, numberOfRounds);
            Console.WriteLine("Player won {0} rounds", gameResults.Count(g => g == Winner.Player));
            Console.WriteLine("Computer won {0} rounds", gameResults.Count(g => g == Winner.Computer));
            Console.WriteLine("No exceptions were thrown");
        }

        private static Winner FindWinner(IBattleshipsBot playerOneBot, IBattleshipsBot playerTwoBot)
        {
            var playerOneShipsPlacement = new ShipsPlacement(playerOneBot);
            var playerTwoShipsPlacement = new ShipsPlacement(playerTwoBot);
            if (!playerOneShipsPlacement.IsValid())
            {
                throw new Exception("Player One Ship Placement Invalid");
            }

            if (!playerTwoShipsPlacement.IsValid())
            {
                throw new Exception("Player Two Ship Placement Invalid");
            }

            while (true)
            {
                MakeMove(playerOneBot, playerTwoBot, playerTwoShipsPlacement);

                if (playerTwoShipsPlacement.AllHit())
                {
                    return Winner.Player;
                }

                MakeMove(playerTwoBot, playerOneBot, playerOneShipsPlacement);

                if (playerOneShipsPlacement.AllHit())
                {
                    return Winner.Computer;
                }
            }
        }

        private static void MakeMove(IBattleshipsBot attacker, IBattleshipsBot defender, ShipsPlacement defendingShips)
        {
            var target = attacker.SelectTarget();
            var defendingIsHit = defendingShips.IsHit(target);
            attacker.HandleShotResult(target, defendingIsHit);
            defender.HandleOpponentsShot(target);
        }

        private static bool IsValid(string userInput)
        {
            try
            {
                var numberOfRounds = Convert.ToInt32(userInput);
                return numberOfRounds >= 1 && numberOfRounds <= 101;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
