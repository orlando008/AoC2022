using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));

            List<(int ElfNumber, int Calories)> elves = new List<(int ElfNumber, int Calories)>();

            int calorie = 0;
            int elfNumber = 1;

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add((elfNumber, calorie));

                    elfNumber += 1;
                    calorie = 0;
                }
                else
                {
                    calorie += Convert.ToInt32(line);
                }
            }

            elves.Add((elfNumber, calorie));

            IEnumerable<(int ElfNumber, int Calories)> enumerableList = elves;

            var elvesSorted = elves.OrderByDescending(e => e.Calories).ToList();

            Console.WriteLine("Biggest boi: Elf #" + elvesSorted[0].ElfNumber.ToString() + ", Calories: " + elvesSorted[0].Calories.ToString());

            Console.WriteLine("Top 3 Elves");
            Console.WriteLine("Elf #" + elvesSorted[0].ElfNumber.ToString() + ", Calories: " + elvesSorted[0].Calories.ToString());
            Console.WriteLine("Elf #" + elvesSorted[1].ElfNumber.ToString() + ", Calories: " + elvesSorted[1].Calories.ToString());
            Console.WriteLine("Elf #" + elvesSorted[2].ElfNumber.ToString() + ", Calories: " + elvesSorted[2].Calories.ToString());

            Console.WriteLine("Total: " + (elvesSorted[0].Calories + elvesSorted[1].Calories + elvesSorted[2].Calories).ToString());
        }
    }
}
