using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        static void Part1()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> linesInFile = totalString.Split("\n").ToList();
            List<ElfSignal> elfSignals = new List<ElfSignal>();

            foreach (string line in linesInFile)
            {
                string commandText = line.Split(" ")[0].Trim();
                int movementAmount = 0;
                if (commandText == "addx")
                {
                    movementAmount = Convert.ToInt32(line.Split(" ")[1].Trim());
                }
                elfSignals.Add(new ElfSignal(commandText, movementAmount));
            }

            int currentCycle = 1;
            int currentX = 1;
            int signalSum = 0;

            while (currentCycle <= 220)
            {
                Console.WriteLine("X at beginning of cycle " + currentCycle.ToString() + "=" + currentX.ToString());

                ElfSignal currentSignal = elfSignals.First(e => !e.CommandFullyExecuted);

                Console.WriteLine("Command: " + currentSignal.CommandText + " " + currentSignal.MovementAmount.ToString());

                currentSignal.CyclesActedUponThisCommand += 1;

                if (currentCycle == 20 || currentCycle == 60 || currentCycle == 100 || currentCycle == 140 || currentCycle == 180 || currentCycle == 220)
                {
                    signalSum += (currentCycle * currentX);
                    Console.WriteLine("Signal strength during " + currentCycle.ToString() + " cycle. " + "Current X = " + currentX.ToString() + ". Signal strength = " + (currentCycle * currentX).ToString());
                }

                if (currentSignal.CommandText == "addx")
                {
                    if (currentSignal.CommandFullyExecuted)
                    {
                        currentX += currentSignal.MovementAmount;
                    }
                }

                Console.WriteLine("X at end of cycle " + currentCycle.ToString() + "=" + currentX.ToString());

                currentCycle += 1;
            }

            Console.WriteLine("Sum of signals = " + signalSum.ToString());
        }

        static void Part2()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> linesInFile = totalString.Split("\n").ToList();
            List<ElfSignal> elfSignals = new List<ElfSignal>();

            foreach (string line in linesInFile)
            {
                string commandText = line.Split(" ")[0].Trim();
                int movementAmount = 0;
                if (commandText == "addx")
                {
                    movementAmount = Convert.ToInt32(line.Split(" ")[1].Trim());
                }
                elfSignals.Add(new ElfSignal(commandText, movementAmount));
            }

            int currentCycle = 1;
            int currentX = 1;
            int signalSum = 0;

            string[,] crtScreen = new string[6,40];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    crtScreen[i, j] = ".";
                }   
            }

            Console.WriteLine("-------------------------------------------------");

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    Console.Write(crtScreen[i, j]);
                }

                Console.WriteLine();
            }

            while (currentCycle <= 240)
            {
                int crtRow = currentCycle / 40;

                int crtColumn = currentCycle - (crtRow * 40) - 1;

                ElfSignal currentSignal = elfSignals.First(e => !e.CommandFullyExecuted);

                if(currentX == crtColumn || currentX == crtColumn - 1 || currentX == crtColumn + 1)
                {
                    crtScreen[crtRow, crtColumn] = "#";
                }
                

                for (int i = 0; i < 6; i++)
                {
                    for(int j = 0; j < 40; j++)
                    {
                        Console.Write(crtScreen[i,j]);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("-------------------------------------------------");

                currentSignal.CyclesActedUponThisCommand += 1;

                if (currentSignal.CommandText == "addx")
                {
                    if (currentSignal.CommandFullyExecuted)
                    {
                        currentX += currentSignal.MovementAmount;
                    }
                }

                currentCycle += 1;
            }

            Console.WriteLine("-------------------------------------------------");

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    Console.Write(crtScreen[i, j]);
                }

                Console.WriteLine();
            }

        }

        class ElfSignal
        {
            public string CommandText;
            public int MovementAmount;
            
            public int CyclesActedUponThisCommand = 0;

            public ElfSignal(string commandText, int movementAmount)
            {
                CommandText = commandText;
                MovementAmount = movementAmount;
            }

            public bool CommandFullyExecuted
            {
                get
                {
                    if(CommandText == "addx")
                    {
                        return CyclesActedUponThisCommand == 2;
                    }
                    else
                    {
                        return CyclesActedUponThisCommand == 1;
                    }
                    
                }
            }
        }
    }
}
