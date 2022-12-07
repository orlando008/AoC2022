using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
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

            string s1, s2, s3, s4;
            for (int i = 0; i < totalString.Length; i++)
            {
                s1 = totalString[i].ToString();
                s2 = totalString[i + 1].ToString();
                s3 = totalString[i + 2].ToString();
                s4 = totalString[i + 3].ToString();

                if ((s1 != s2 && s1 != s3 && s1 != s4)
                   &&
                   (s2 != s1 && s2 != s3 && s2 != s4)
                   &&
                   (s3 != s1 && s3 != s2 && s3 != s4)
                   &&
                   (s4 != s1 && s4 != s2 && s4 != s3))
                {
                    Console.Write("Found unique sequence at " + (i + 4).ToString());
                    break;
                }
            }
        }

        static void Part2()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            for (int i = 0; i < totalString.Length; i++)
            {
                List<string> newChunk = new List<string>();
                for(int j = 0; j < 14; j++)
                {
                    newChunk.Add(totalString[i+j].ToString());
                }

                bool foundDuplicate = false;
                foreach(string s in newChunk)
                {
                    int charCount = newChunk.Count(c => c == s);
                    if(charCount > 1)
                    {
                        foundDuplicate = true;
                        break;
                    }
                }

                if (foundDuplicate)
                    continue;
                else
                {
                    Console.Write("Found unique sequence at " + (i + 14).ToString());
                    break;
                }

            }
        }
    }
}
