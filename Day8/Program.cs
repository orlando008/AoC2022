using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
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

            var rows = totalString.Split("\n");
            List<ElfTree> trees = new List<ElfTree>();

            for (int row = 0; row < 99; row++)
            {
                for(int col = 0; col < 99; col++)
                {
                    trees.Add(new ElfTree(Convert.ToInt32(rows[row].ToCharArray()[col].ToString()), row, col));
                }
            }

            int totalVisible = 0;
            int maxScenicScore = 0;

            foreach(ElfTree tree in trees)
            {
                if(IsTreeVisible(tree, trees))
                {
                    tree.IsVisible = true;
                    
                    Console.WriteLine("Found tree at Row(" + tree.Row + "), Column (" + tree.Column + ")");
                    totalVisible += 1;
                }

                int scenicScore = GetScenicScore(tree, trees);
                if(scenicScore > maxScenicScore)
                {
                    maxScenicScore = scenicScore;
                }
            }

            Console.WriteLine("Total visibles: " + totalVisible.ToString());
            Console.WriteLine("Max scenic score: " + maxScenicScore.ToString());
        }

        static bool IsTreeVisible(ElfTree tree, List<ElfTree> otherTrees)
        {
            List<ElfTree> treesInSameRow = new List<ElfTree>(otherTrees.Where(t => t.Row == tree.Row && t != tree));
            List<ElfTree> treesInSameColumn = new List<ElfTree>(otherTrees.Where(t => t.Column == tree.Column && t != tree));

            if(tree.Column == 0 || tree.Column == 98)
            {
                return true;
            }
            else if(tree.Row == 0 || tree.Row == 98)
            {
                return true;
            }

            if(IsTreeVisibleFromTheLeft(tree, treesInSameRow))
            {
                return true;
            }
            else if(IsTreeVisibleFromTheRight(tree, treesInSameRow))
            {
                return true;
            }
            else if(IsTreeVisibleFromTheTop(tree, treesInSameColumn))
            {
                return true;
            }
            else if(IsTreeVisibleFromTheBottom(tree, treesInSameColumn))
            {
                return true;
            }

            return false;
        }

        static int GetScenicScore(ElfTree tree, List<ElfTree> otherTrees)
        {
            List<ElfTree> treesInSameRow = new List<ElfTree>(otherTrees.Where(t => t.Row == tree.Row && t != tree));
            List<ElfTree> treesInSameColumn = new List<ElfTree>(otherTrees.Where(t => t.Column == tree.Column && t != tree));

            int left = GetNumberOfTreesVisibleToTheLeft(tree, treesInSameRow);
            int right = GetNumberOfTreesVisibleToTheRight(tree, treesInSameRow);
            int up = GetNumberOfTreesVisibleAbove(tree, treesInSameColumn);
            int down = GetNumberOfTreesVisibleBelow(tree, treesInSameColumn);

            return left * right * up * down;
        }

        static int GetNumberOfTreesVisibleToTheLeft(ElfTree tree, List<ElfTree> treesInSameRow)
        {
            List<ElfTree> treesToTheLeft = new List<ElfTree>(treesInSameRow.Where(t => t.Column < tree.Column).OrderByDescending(t => t.Column));
            return TreesSeen(tree, treesToTheLeft);
        }

        static int GetNumberOfTreesVisibleToTheRight(ElfTree tree, List<ElfTree> treesInSameRow)
        {
            List<ElfTree> treesToTheRight = new List<ElfTree>(treesInSameRow.Where(t => t.Column > tree.Column).OrderBy(t => t.Column));
            return TreesSeen(tree, treesToTheRight);
        }

        static int GetNumberOfTreesVisibleAbove(ElfTree tree, List<ElfTree> treesInSameColumn)
        {
            List<ElfTree> treesToUp = new List<ElfTree>(treesInSameColumn.Where(t => t.Row < tree.Row).OrderByDescending(t => t.Row));
            return TreesSeen(tree, treesToUp);
        }

        static int GetNumberOfTreesVisibleBelow(ElfTree tree, List<ElfTree> treesInSameColumn)
        {
            List<ElfTree> treesBelow = new List<ElfTree>(treesInSameColumn.Where(t => t.Row > tree.Row).OrderBy(t => t.Row));
            return TreesSeen(tree, treesBelow);
        }

        static int TreesSeen(ElfTree tree, List<ElfTree> otherTreesToCheckInOrder)
        {
            int treesSeen = 0;

            foreach (ElfTree t in otherTreesToCheckInOrder)
            {
                treesSeen += 1;
                if (t.Height >= tree.Height)
                {
                    break;
                }
            }

            return treesSeen;
        }

        static bool IsTreeVisibleFromTheLeft(ElfTree tree, List<ElfTree> treesInSameRow)
        {
            if (treesInSameRow.Any(t => t.Column < tree.Column && t.Height >= tree.Height))
                return false;
            else
                return true;
        }

        static bool IsTreeVisibleFromTheRight(ElfTree tree, List<ElfTree> treesInSameRow)
        {
            if (treesInSameRow.Any(t => t.Column > tree.Column && t.Height >= tree.Height))
                return false;
            else
                return true;
        }

        static bool IsTreeVisibleFromTheTop(ElfTree tree, List<ElfTree> treesInSameColumn)
        {
            if (treesInSameColumn.Any(t => t.Row < tree.Row && t.Height >= tree.Height))
                return false;
            else
                return true;
        }

        static bool IsTreeVisibleFromTheBottom(ElfTree tree, List<ElfTree> treesInSameColumn)
        {
            if (treesInSameColumn.Any(t => t.Row > tree.Row && t.Height >= tree.Height))
                return false;
            else
                return true;
        }

        class ElfTree
        {
            public int Height;
            public int Row;
            public int Column;
            public bool IsVisible = false;

            public ElfTree(int height, int row, int column)
            {
                Height = height;
                Row = row;
                Column = column;
            }

        }
    }
}
