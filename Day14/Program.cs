using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Day14
{
    internal class Program
    {
        const int ROWS = 165;
        const int COLUMNS = 780;

        static void Main(string[] args)
        {
            string[,] cave = new string[ROWS, COLUMNS];

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    cave[row, col] = ".";

                    if (row == ROWS - 1)
                    {
                        cave[row, col] = "#";
                    }
                }     
            }

            cave[0, 500] = "+";

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var moves = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
   
                    int prevCol = -1;
                    int prevRow = -1;

                    foreach(string move in moves)
                    {
                        int col = Convert.ToInt32(move.Split(",")[0].Trim());
                        int row = Convert.ToInt32(move.Split(",")[1].Trim());
                        cave[row, col] = "#";

                        if(prevCol != -1)
                        {
                            if(prevCol != col)
                            {
                                //do all row values between the columns
                                for (int i = 1; i < Math.Abs(prevCol - col); i++)
                                {
                                    if(prevCol < col)
                                    {
                                        cave[row, prevCol + i] = "#";
                                    }
                                    else
                                    {
                                        cave[row, prevCol - i] = "#";
                                    }
                                }
                            }
                            else
                            {
                                //do all the column values between the rows
                                for (int i = 1; i < Math.Abs(prevRow - row); i++)
                                {
                                    if(prevRow < row)
                                    {
                                        cave[prevRow + i, col] = "#";
                                    }
                                    else
                                    {
                                        cave[prevRow - i, col] = "#";
                                    }
                                }
                            }
                        }

                        prevCol = col;
                        prevRow = row;
                    }

                }
            }

            PrintCave(cave, 0, ROWS);

            (int col, int row) currentSand = (500, 0);

            int runcount = 0;
            int totalSandPlace = 0;

            while (true)
            {

                string blockBelow = cave[currentSand.row + 1, currentSand.col];
                string blockLeft = cave[currentSand.row + 1, currentSand.col - 1];
                string blockRight = cave[currentSand.row + 1, currentSand.col + 1];

                if(blockLeft == "O" && blockRight == "O" && blockRight == "O" && currentSand.row == 0)
                {
                    PrintCave(cave, 0, ROWS);
                    break;
                }

                if (blockBelow == ".")
                {
                    currentSand.row += 1;
                }
                else if (blockLeft == ".")
                {
                    currentSand.col -= 1;
                    currentSand.row += 1;
                }
                else if (blockRight == ".")
                {
                    currentSand.col += 1;
                    currentSand.row += 1;
                }
                else
                {
                    cave[currentSand.row, currentSand.col] = "O";
                    currentSand.col = 500;
                    currentSand.row = 0;

                    totalSandPlace += 1;
                }

                runcount += 1;
            }

            Console.WriteLine("Total sand placed: " + totalSandPlace.ToString());
           
        }

        static void PrintCave(string[,] cave, int startingColumn, int endingRow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("   ");
            for (int i = startingColumn; i < COLUMNS; i++)
            {
                sb.Append(i.ToString().PadLeft(3, '0')[0].ToString());
            }

            sb.AppendLine();
            sb.Append("   ");
            for (int i = startingColumn; i < COLUMNS; i++)
            {
                sb.Append(i.ToString().PadLeft(3, '0')[1].ToString());
            }

            sb.AppendLine();
            sb.Append("   ");

            for (int i = startingColumn; i < COLUMNS; i++)
            {
                sb.Append(i.ToString().PadLeft(3, '0')[2].ToString());
            }

            sb.AppendLine();

            for (int i = 0; i < endingRow; i++)
            {
                sb.Append(i.ToString().PadRight(3));

                for (int j = startingColumn; j < COLUMNS; j++)
                {
                    sb.Append(cave[i, j]); 
                }

                sb.AppendLine();

            }

            //Console.WriteLine(sb.ToString());
            LogToOutput(sb.ToString());
        }

        static void LogToOutput(string s)
        {
            StreamWriter sw = new StreamWriter("output.txt");
            sw.WriteLine(s);
            sw.Close();
        }
    }
}
