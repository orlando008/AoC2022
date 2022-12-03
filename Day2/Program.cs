using System;
using System.Collections.Generic;
using System.IO;

namespace Day2
{
    class Program
    {
        private enum RockPaperScissorsEnum
        {
            Tie = 0,
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum OutcomeEnum
        {
            Lose = 0,
            Win = 1,
            Draw = 2
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Part 1 or 2?");
            string input = Console.ReadLine();

            if(input == "1")
            {
                Part1();
            }
            else
            {
                Part2();
            }      
        }

        private static void Part1()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));

            int totalScore = 0;

            foreach (string roundString in lines)
            {

                RockPaperScissorsEnum opponentPlay = PlayName(roundString[0].ToString());
                RockPaperScissorsEnum myPlay = PlayName(roundString[2].ToString());

                string myPlayName = Enum.GetName(typeof(RockPaperScissorsEnum), myPlay);
                string opponentPlayName = Enum.GetName(typeof(RockPaperScissorsEnum), opponentPlay);

                RockPaperScissorsEnum winner = DeclareWinner(myPlay, opponentPlay);

                Console.Write(myPlayName.PadRight(10) + "\t vs.\t" + opponentPlayName.PadRight(10) + "\t Winner: \t");

                if (winner == RockPaperScissorsEnum.Tie)
                {
                    Console.Write("TIE".PadRight(10));
                }
                else
                {
                    if (winner == myPlay)
                    {
                        Console.Write("ME".PadRight(10));
                    }
                    else
                    {
                        Console.Write("OPPONENT".PadRight(10));
                    }
                }

                int points = (int)myPlay;
                if (winner == myPlay)
                {
                    points += 6;
                }
                else if (winner == RockPaperScissorsEnum.Tie)
                {
                    points += 3;
                }

                Console.WriteLine("\t Points Awarded: " + points.ToString());

                totalScore += points;
            }

            Console.WriteLine("Total Tournament Score: " + totalScore);
        }

        private static void Part2()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));

            int totalScore = 0;

            foreach (string roundString in lines)
            {

                OutcomeEnum desiredOutcome = OutcomeName(roundString[2].ToString());
                RockPaperScissorsEnum opponentPlay = PlayName(roundString[0].ToString());
                
                string desiredOutcomeName = Enum.GetName(typeof(OutcomeEnum), desiredOutcome);
                string opponentPlayName = Enum.GetName(typeof(RockPaperScissorsEnum), opponentPlay);
        
                Console.Write(opponentPlayName.PadRight(10) + "\t Desired Outcome:\t" + desiredOutcomeName.PadRight(10) + "\t Play Should Be: \t");

                RockPaperScissorsEnum myPlay = opponentPlay;

                if(desiredOutcome == OutcomeEnum.Win)
                {
                    if (opponentPlay == RockPaperScissorsEnum.Rock)
                    {
                        myPlay = RockPaperScissorsEnum.Paper;
                    }
                    else if (opponentPlay == RockPaperScissorsEnum.Paper)
                    {
                        myPlay = RockPaperScissorsEnum.Scissors;
                    }
                    else
                    {
                        myPlay = RockPaperScissorsEnum.Rock;
                    }
                }
                else if(desiredOutcome == OutcomeEnum.Lose)
                {
                    if (opponentPlay == RockPaperScissorsEnum.Rock)
                    {
                        myPlay = RockPaperScissorsEnum.Scissors;
                    }
                    else if (opponentPlay == RockPaperScissorsEnum.Paper)
                    {
                        myPlay = RockPaperScissorsEnum.Rock;
                    }
                    else
                    {
                        myPlay = RockPaperScissorsEnum.Paper;
                    }
                }



                string myPlayName = Enum.GetName(typeof(RockPaperScissorsEnum), myPlay); ;
                Console.Write(myPlayName.PadRight(10));

                int points = (int)myPlay;

                if (desiredOutcome == OutcomeEnum.Win)
                {
                    points += 6;
                }
                else if (desiredOutcome == OutcomeEnum.Draw)
                {
                    points += 3;
                }

                Console.WriteLine("\t Points Awarded: " + points.ToString());

                totalScore += points;
            }

            Console.WriteLine("Total Tournament Score: " + totalScore);
        }

        private static RockPaperScissorsEnum DeclareWinner(RockPaperScissorsEnum play1, RockPaperScissorsEnum play2)
        {
            if(play1 == play2)
            {
                return RockPaperScissorsEnum.Tie;
            }

            if(play1 == RockPaperScissorsEnum.Rock)
            {
                if(play2 == RockPaperScissorsEnum.Paper)
                {
                    return play2;
                }
                else
                {
                    return play1;
                }
            }

            if (play1 == RockPaperScissorsEnum.Paper)
            {
                if (play2 == RockPaperScissorsEnum.Scissors)
                {
                    return play2;
                }
                else
                {
                    return play1;
                }
            }

            if (play1 == RockPaperScissorsEnum.Scissors)
            {
                if (play2 == RockPaperScissorsEnum.Rock)
                {
                    return play2;
                }
                else
                {
                    return play1;
                }
            }

            return RockPaperScissorsEnum.Tie;
        }

        private static RockPaperScissorsEnum PlayName(string play)
        {
            switch (play)
            {
                case "A":
                case "X":
                    return RockPaperScissorsEnum.Rock;
                case "B":
                case "Y":
                    return RockPaperScissorsEnum.Paper;
                case "C":
                case "Z":
                    return RockPaperScissorsEnum.Scissors;
            }

            return RockPaperScissorsEnum.Tie;
        }

        private static OutcomeEnum OutcomeName(string outcome)
        {
            switch (outcome)
            {
                case "X":
                    return OutcomeEnum.Lose;
                case "Y":
                    return OutcomeEnum.Draw;
                case "Z":
                    return OutcomeEnum.Win;
            }

            return OutcomeEnum.Win;
        }
    }
}
