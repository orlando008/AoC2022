using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Day9
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

            List<string> linesInFile = totalString.Split("\n").ToList();
            Part2(linesInFile);
        }

        private static void Part1(List<string> linesInFile)
        {
            int headXPosition = 0;
            int headYPosition = 0;
            int tailXPosition = 0;
            int tailYPosition = 0;

            //How many positions does the tail of the rope visit at least once?
            List<Point> pointsVisited = new List<Point>();
            pointsVisited.Add(new Point(0, 0));

            StreamWriter sw = new StreamWriter("output.txt");

            foreach (string line in linesInFile)
            {
                string direction = line.Split(" ")[0];
                int amount = Convert.ToInt32(line.Split(" ")[1]);

                sw.WriteLine("Move " + direction + " " + amount.ToString());

                for (int i = 0; i < amount; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            //up
                            headYPosition += 1;
                            break;
                        case "D":
                            //down
                            headYPosition -= 1;
                            break;
                        case "L":
                            //left
                            headXPosition -= 1;
                            break;
                        case "R":
                            //right
                            headXPosition += 1;
                            break;
                    }

                    Console.WriteLine("Head at " + headXPosition.ToString() + ", " + headYPosition.ToString());
                    sw.WriteLine("Head at " + headXPosition.ToString() + ", " + headYPosition.ToString());
                    sw.WriteLine("Tail at " + tailXPosition.ToString() + ", " + tailYPosition.ToString());

                    //Is tail close enough?
                    if (!AreHeadAndTailCloseEnough(headXPosition, headYPosition, tailXPosition, tailYPosition))
                    {
                        //Move tail
                        Console.WriteLine("Tail needs to move");
                        sw.WriteLine("*** TAIL MOVE NEEDED *** ");
                        Point p = GetTailPointToMoveTo(headXPosition, headYPosition, tailXPosition, tailYPosition);
                        tailXPosition = p.X;
                        tailYPosition = p.Y;

                        if (!pointsVisited.Any(c => c.X == p.X && c.Y == p.Y))
                        {
                            sw.Write("New Point visited");
                            pointsVisited.Add(p);
                        }

                        sw.WriteLine("Tail NOW AT " + tailXPosition.ToString() + ", " + tailYPosition.ToString());
                    }
                }
            }

            sw.WriteLine("Answer: " + pointsVisited.Count.ToString() + " points visited.");
            Console.WriteLine("Answer: " + pointsVisited.Count.ToString() + " points visited.");
            sw.Close();
            sw.Dispose();
        }

        private static void Part2(List<string> linesInFile)
        {
            Dictionary<int, Point> knots = new Dictionary<int, Point>();
            for (int i = 0; i <= 9; i++)
            {
                knots.Add(i, new Point(0, 0));
            }

            //How many positions does the knot 9 of the rope visit at least once?
            List<Point> pointsVisited = new List<Point>();
            pointsVisited.Add(new Point(0, 0));

            StreamWriter sw = new StreamWriter("output.txt");

            foreach (string line in linesInFile)
            {
                string direction = line.Split(" ")[0];
                int amount = Convert.ToInt32(line.Split(" ")[1]);

                sw.WriteLine("Move " + direction + " " + amount.ToString());

                for (int i = 0; i < amount; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            //up
                            knots[0] = new Point(knots[0].X, knots[0].Y + 1);
                            break;
                        case "D":
                            //down
                            knots[0] = new Point(knots[0].X, knots[0].Y - 1);
                            break;
                        case "L":
                            //left
                            knots[0] = new Point(knots[0].X - 1, knots[0].Y);
                            break;
                        case "R":
                            //right
                            knots[0] = new Point(knots[0].X + 1, knots[0].Y);
                            break;
                    }

                    Console.WriteLine("Head at " + knots[0].X.ToString() + ", " + knots[0].Y.ToString());

                    for (int knotID = 1; knotID <= 9; knotID++)
                    {
                        //Is tail close enough?
                        if (!AreHeadAndTailCloseEnough(knots[knotID-1].X, knots[knotID - 1].Y, knots[knotID].X, knots[knotID].Y))
                        {
                            //Move tail
                            Console.WriteLine("Tail needs to move");
                            sw.WriteLine("*** TAIL MOVE NEEDED *** ");
                            Point p = GetTailPointToMoveTo(knots[knotID - 1].X, knots[knotID - 1].Y, knots[knotID].X, knots[knotID].Y);
                            knots[knotID] = p;

                            if(knotID == 9)
                            {
                                if (!pointsVisited.Any(c => c.X == p.X && c.Y == p.Y))
                                {
                                    sw.Write("New Point visited");
                                    pointsVisited.Add(p);
                                }
                            }
                        }
                    }

  
                }
            }

            sw.WriteLine("Answer: " + pointsVisited.Count.ToString() + " points visited.");
            Console.WriteLine("Answer: " + pointsVisited.Count.ToString() + " points visited.");
            sw.Close();
            sw.Dispose();
        }

        private static List<Point> GetListOfPointsSurroundingPoint(int x1, int y1)
        {
            List<Point> pointsSurroundingHead = new List<Point>();

            //West, East, North, South
            pointsSurroundingHead.Add(new Point(x1 - 1, y1));
            pointsSurroundingHead.Add(new Point(x1 + 1, y1));
            pointsSurroundingHead.Add(new Point(x1, y1 + 1));
            pointsSurroundingHead.Add(new Point(x1, y1 - 1));

            //SouthWest, SouthEast, NorthWest, NorthEast
            pointsSurroundingHead.Add(new Point(x1 - 1, y1 - 1));
            pointsSurroundingHead.Add(new Point(x1 + 1, y1 - 1));
            pointsSurroundingHead.Add(new Point(x1 - 1, y1 + 1));
            pointsSurroundingHead.Add(new Point(x1 + 1, y1 + 1));

            return pointsSurroundingHead;
        }

        private static Point GetTailPointToMoveTo(int headX, int headY, int tailX, int tailY)
        {
            List<Point> pointsSurroundingHead = GetListOfPointsSurroundingPoint(headX, headY);
            List<Point> pointsSurroundingTail = GetListOfPointsSurroundingPoint(tailX, tailY);

            if(headX == tailX || headY == tailY)
            {
                foreach (Point p in pointsSurroundingTail)
                {
                    if (pointsSurroundingHead.Any(c => c.X == p.X && c.Y == p.Y && (p.X == tailX || p.Y == tailY)))
                    {
                        return p;
                    }
                }
            }
            else
            {
                foreach (Point p in pointsSurroundingTail)
                {
                    if (pointsSurroundingHead.Any(c => c.X == p.X && c.Y == p.Y && p.X != tailX && p.Y != tailY))
                    {
                        return p;
                    }
                }
            }

   

            return new Point(tailX, tailY);
        }

        private static bool AreHeadAndTailCloseEnough(int x1, int y1, int x2, int y2)
        {
            if(x1 == x2 && y1 == y2)
            {
                return true;
            }

            List<Point> pointsSurroundingHead = GetListOfPointsSurroundingPoint(x1, y1);

            if (pointsSurroundingHead.Any(c => c.X == x2 && c.Y == y2))
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
