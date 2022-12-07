using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
            List<CrateStack> stacks = GetStacks();


            foreach(CrateStack cs in stacks)
            {
                cs.PrintStack();
            }

            List<Move> moves = GetMoves();

            Part1(stacks, moves);

            string answer1 = "";
            foreach (CrateStack cs in stacks)
            {
                cs.PrintStack();
                answer1 += cs.Crates[cs.Crates.Count - 1];
            }

            Console.WriteLine(answer1.Replace("[", "").Replace("]", ""));

            Console.WriteLine("Part 2 --------");
            stacks.Clear();
            stacks = GetStacks();
            Part2(stacks, moves);

            string answer2 = "";
            foreach (CrateStack cs in stacks)
            {
                cs.PrintStack();
                answer2 += cs.Crates[cs.Crates.Count - 1];
            }

            Console.WriteLine(answer2.Replace("[", "").Replace("]", ""));
        }

        static List<Move> GetMoves()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));
            List<Move> moves = new List<Move>();

            foreach (string line in lines)
            {
                if (line.StartsWith("move"))
                {
                    Move m = new Move();
                    m.NumberToMove = Convert.ToInt32(line.Substring(5, line.IndexOf("from")-5).Trim());
                    m.StackSource = Convert.ToInt32(line.Substring(line.IndexOf("from") + 4, 3).Trim());
                    m.StackDestination = Convert.ToInt32(line.Substring(line.IndexOf("to")+2).Trim());

                    moves.Add(m);
                }
            }

            return moves;
        }


        static List<CrateStack> GetStacks()
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            List<string> lines = new List<string>(totalString.Split("\n"));

            List<CrateStack> stacks = new List<CrateStack>();

            for (int i = 1; i <= 9; i++)
            {
                CrateStack cs = new CrateStack();
                cs.StackNumber = i;
                cs.Crates = new List<string>();

                stacks.Add(cs);
            }
            
            foreach (string line in lines)
            {
                if (!line.StartsWith("["))
                    break;

                foreach(CrateStack cs in stacks)
                {
                    var s = line.Substring(((cs.StackNumber - 1) * 4), 4).Trim();
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        cs.Crates.Insert(0,s);
                    }
                }     
            }
            
            return stacks;
        }

        static void Part1(List<CrateStack> stacks, List<Move> moves)
        {
            foreach(Move m in moves)
            {
                CrateStack csSource = stacks[m.StackSource - 1];
                CrateStack csDest = stacks[m.StackDestination - 1];

                for(int i = 0; i < m.NumberToMove; i++)
                {
                    string crateToMove = csSource.Crates[csSource.Crates.Count-1];
                    csDest.Crates.Add(crateToMove);
                    csSource.Crates.RemoveAt(csSource.Crates.Count - 1);
                    Console.WriteLine("Moving " + crateToMove + " from " + csSource.StackNumber.ToString() + " to " + csDest.StackNumber.ToString());
                }
            }
        }

        static void Part2(List<CrateStack> stacks, List<Move> moves)
        {
            foreach (Move m in moves)
            {
                CrateStack csSource = stacks[m.StackSource - 1];
                CrateStack csDest = stacks[m.StackDestination - 1];

                int numberMoved = 0;

                for (int i = 0; i < m.NumberToMove; i++)
                {
                    string crateToMove = csSource.Crates[csSource.Crates.Count - (m.NumberToMove - numberMoved)];
                    csDest.Crates.Insert(csDest.Crates.Count, crateToMove);
                    csSource.Crates.RemoveAt(csSource.Crates.Count - (m.NumberToMove - numberMoved));
                    Console.WriteLine("Moving " + crateToMove + " from " + csSource.StackNumber.ToString() + " to " + csDest.StackNumber.ToString());
                    numberMoved += 1;
                }
            }
        }
    }

    class CrateStack
    {
        public int StackNumber;
        public List<string> Crates;

        public void PrintStack()
        {
            Console.WriteLine("Stack: " + StackNumber.ToString());
            Console.WriteLine("Bottom");
            foreach (string c in Crates)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("Top");
        }
    }

    class Move
    {
        public int NumberToMove;
        public int StackSource;
        public int StackDestination;
    }
}
