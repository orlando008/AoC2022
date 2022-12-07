using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
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

            int totalFullyOverlaps = 0;

            foreach (string ranges in lines)
            {
                Console.Write(ranges.Trim());

                string range1 = ranges.Split(",")[0].Trim();
                string range2 = ranges.Split(",")[1].Trim();

                (int x, int y) range1Obj = (Convert.ToInt32(range1.Split("-")[0]), Convert.ToInt32(range1.Split("-")[1]));
                (int x, int y) range2Obj = (Convert.ToInt32(range2.Split("-")[0]), Convert.ToInt32(range2.Split("-")[1]));

                if (DoesRangeOverlapOtherRange(range1Obj, range2Obj))
                {
                    Console.WriteLine(", OVERLAPS");
                    totalFullyOverlaps += 1;
                }
                else
                {
                    Console.WriteLine(", DOES NOT OVERLAP");
                }
            }

            Console.WriteLine("Total partial overlaps: " + totalFullyOverlaps.ToString());
        }

        private static void Part1()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));

            int totalFullyOverlaps = 0;

            foreach(string ranges in lines)
            {
                Console.Write(ranges.Trim());

                string range1 = ranges.Split(",")[0].Trim();
                string range2 = ranges.Split(",")[1].Trim();

                (int x, int y) range1Obj = (Convert.ToInt32(range1.Split("-")[0]), Convert.ToInt32(range1.Split("-")[1]));
                (int x, int y) range2Obj = (Convert.ToInt32(range2.Split("-")[0]), Convert.ToInt32(range2.Split("-")[1]));

                if(DoesRangeFullyContainerOtherRange(range1Obj, range2Obj))
                {
                    Console.WriteLine(", FULLY OVERLAPS");
                    totalFullyOverlaps += 1;
                }
                else
                {
                    Console.WriteLine(", DOES NOT FULLY OVERLAP");
                }
            }

            Console.WriteLine("Total full overlaps: " + totalFullyOverlaps.ToString());
        }

        private static bool DoesRangeFullyContainerOtherRange((int x, int y) r1, (int x, int y) r2)
        {
            if (r1.x <= r2.x && r1.y >= r2.y)
            {
                return true;
            }
            else if (r2.x <= r1.x && r2.y >= r1.y)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private static bool DoesRangeOverlapOtherRange((int x, int y) r1, (int x, int y) r2)
        {
            if (r1.x >= r2.x && r1.x <= r2.y)
            {
                return true;
            }
            else if (r1.y >= r2.x && r1.y <= r2.y)
            {
                return true;
            }
            else if (r2.y >= r1.x && r2.y <= r1.y)
            {
                return true;
            }
            else if (r2.y >= r1.x && r2.y <= r1.y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
