using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
            
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

            string rucksack1 = "";
            string rucksack2 = "";
            string rucksack3 = "";

            for (int i = 0; i < lines.Count-1; i=i+3)
            {
                rucksack1 = lines[i];
                rucksack2 = lines[i+1];
                rucksack3 = lines[i+2];

                foreach (char c in rucksack1)
                {
                    var foundChar = rucksack2.FirstOrDefault(c2 => c2 == c);
                    if (foundChar == 0)
                        continue;
                    else
                    {
                        foundChar = rucksack3.FirstOrDefault(c2 => c2 == c);

                        if (foundChar == 0)
                            continue;
                        else
                        {
                            Console.WriteLine(foundChar.ToString() + ", priority value = " + GetPriorityValue(foundChar).ToString());
                            int priorityVal = GetPriorityValue(foundChar);

                            totalScore += priorityVal;
                            break;
                        }           
                    }
                }
            }

            Console.WriteLine("TOTAL: " + totalScore);
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

            foreach (string ruckSacks in lines)
            {
                string compartment1 = ruckSacks.Substring(0, ruckSacks.Length / 2);
                string compartment2 = ruckSacks.Substring(ruckSacks.Length / 2, ruckSacks.Length / 2);

                foreach (char c in compartment1)
                {
                    var foundChar = compartment2.FirstOrDefault(c2 => c2 == c);
                    if (foundChar == 0)
                        continue;
                    else
                    {
                        Console.WriteLine(foundChar.ToString() + ", priority value = " + GetPriorityValue(foundChar).ToString());
                        totalScore += GetPriorityValue(foundChar);
                        break;
                    }

                }

                Console.WriteLine(totalScore.ToString());
            }
        }

        private static int GetPriorityValue(char c)
        {
            if(char.IsLower(c))
            {
                return c - 96;
            }
            else
            {
                return c - 38;
            }
           
        }
    }
}
