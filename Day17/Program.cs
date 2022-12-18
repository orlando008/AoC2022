using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace Day17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Calculation must be off slightly, but it's something like this based on the repeating pattern.
            long answer = (2668 + 2730) + ((long)581395347 * (long)2729) + (long)2290;
            Console.WriteLine(answer);

            string jetCommands = "";

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                jetCommands = sr.ReadToEnd();
            }

            string originalJetCommands = jetCommands;

            string currentCommand = "new";
            ChamberOfRocks chamber = new ChamberOfRocks();
            Rock currentRock = null;

            int rocksThatHaveStopped = 0;

            int iteration = 0;


            while (jetCommands != "")
            {
                if(currentCommand == "new")
                {
                    currentRock = GenerateNewRock(currentRock, chamber);
                    chamber.Rocks.Add(currentRock);
                    currentCommand = "gasmove";
                }
                else if(currentCommand == "fall")
                {
                    bool itFellSuccessfully = PerformFall(currentRock);

                    if(!itFellSuccessfully)
                    {
                        currentRock.IsInMotion = false;
                        currentCommand = "new";
                        rocksThatHaveStopped += 1;

                        if(iteration > 3 && rocksThatHaveStopped == 1445)
                        {
                            break;
                        }
                    }
                    else
                    {
                        currentCommand = "gasmove";
                    }
                }
                else
                {
                    currentCommand = jetCommands.Substring(0, 1);
                    PerformGasMove(currentRock, currentCommand);

                    if(jetCommands.Length == 1)
                    {
                        jetCommands = originalJetCommands;

                        iteration += 1;

                        Console.WriteLine("Rocks stopped after " + iteration + " commands: " + rocksThatHaveStopped);
                        Console.WriteLine("Top of tower at " + currentRock.Position.Y);
                        rocksThatHaveStopped = 0;

                        chamber.PruneChamber();

                        Console.WriteLine("Top of tower now at " + currentRock.Position.Y);
                    }
                    else
                    {
                        jetCommands = jetCommands.Substring(1);
                    }

                    currentCommand = "fall";
                }

            }

           // Console.WriteLine(chamber);
            Console.WriteLine("Top of tower at " + currentRock.Position.Y);
        }
        
        static Rock GenerateNewRock(Rock previousRock, ChamberOfRocks chamber)
        {
            int positionOfFloor = 0;

            if (previousRock != null)
            {
                positionOfFloor = previousRock.Position.Y;
            }
            
            if(chamber.Rocks == null)
            {
                chamber.Rocks = new List<Rock>();
            }

            if (previousRock == null || previousRock.GetType() == typeof(FourClumpRock))
            {
                return new MinusRock(chamber);
            }
            else if (previousRock.GetType() == typeof(MinusRock))
            {
                return new PlusRock(chamber);
            }
            else if (previousRock.GetType() == typeof(PlusRock))
            {
                return new BackwardsLShapeRock(chamber);
            }
            else if (previousRock.GetType() == typeof(BackwardsLShapeRock))
            {
                return new VertlineRock(chamber);
            }
            else
            {
                return new FourClumpRock(chamber);
            }
        }

        static bool PerformFall(Rock rock)
        {
            if (rock.CanMoveInDirection(0, -1))
            {
                rock.MoveRockDown();
                return true;
            }
            else
            {
                return false;
            }
        }

        static void PerformGasMove(Rock rock, string gasCommand)
        {
            rock.MoveRockBecauseOfGas(gasCommand);
        }
        
        class ChamberOfRocks
        {
            public List<Rock> Rocks;
            public int CHAMBER_WIDTH = 9;

            public int PositionOfFloor
            {
                get
                {
                    if (Rocks == null || Rocks.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return Rocks.Max(r => r.Position.Y);
                    }
                }
            }

            public void PruneChamber()
            {
                int newFloor = GetNewBaseLineRow();

                Console.WriteLine("New floor = " + newFloor);
                if(newFloor != 0)
                {
                    Rocks.RemoveAll(r => r.Position.Y <= newFloor);

                    foreach (Rock rock in Rocks)
                    {
                        rock.Position.Offset(0, -newFloor);
                    }
                }
            }

            public int GetNewBaseLineRow()
            {
                bool everyColumnFilled = true;

                for (int row = GetTopOfTower(); row > 0; row--)
                {
                    everyColumnFilled = true;

                    for (int column = 1; column < CHAMBER_WIDTH - 1; column++)
                    {
                
                        Rock rockAtPoint = this.IsThereARockAtPoint(new Point(column, row));
                        if (rockAtPoint == null)
                        {
                            everyColumnFilled = false;
                            break;
                        }
                    }

                    if (everyColumnFilled)
                    {
                        return row;
                    }
                }

                return 0;
            }

            public Rock IsThereARockAtPoint(Point p)
            {
                return Rocks.FirstOrDefault(r => r.GetAllPoints().Any(r1 => r1.X == p.X && r1.Y == p.Y));
            }

            public int GetTopOfTower()
            {
                return Rocks.Max(r => r.Position.Y);
            }

            public override string ToString()
            {
                string s = "";

                for (int row = GetTopOfTower(); row >= 0; row--)
                {
                    for (int column = 0; column < CHAMBER_WIDTH; column++)
                    {
                        if(column == 0 || column == CHAMBER_WIDTH - 1)
                        {
                            s += "|";
                        }
                        else if(row == 0)
                        {
                            s += "-";
                        }
                        else
                        {
                            Rock rockAtPoint = this.IsThereARockAtPoint(new Point(column, row));
                            if(rockAtPoint != null)
                            {
                                if (rockAtPoint.IsInMotion)
                                {
                                    s += "@";
                                }
                                else
                                {
                                    s += "#";
                                }  
                            }
                            else
                            {
                                s += ".";
                            }
                        }
                    }

                    s += "\n";
                }

                return s;
            }
        }

        class Rock
        {
            public enum RockTypeEnum
            {
                MINUS = 0,
                PLUS = 1,
                BACKWARDSL = 2,
                VERTLINE = 3,
                FOURCLUMP = 4
            }

            public Point Position;
            public int PositionOfFloor;
            public bool IsInMotion = true;
            public ChamberOfRocks ParentChamber;

            public Rock(ChamberOfRocks parentChamber)
            {
                this.ParentChamber = parentChamber;
            }

            public void MoveRockDown()
            {
                Position.Offset(0, -1);
            }


            public bool PerformFall()
            {
                if(CanMoveInDirection(0, -1))
                {
                    this.MoveRockDown();
                    return true;
                }
                else
                {
                    this.IsInMotion = false;
                    return false;
                }
            }

            public bool CanMoveInDirection(int dx, int dy)
            {
                foreach (Point p in GetAllPoints())
                {
                    if (p.X + dx <= 0 || p.X + dx >= ParentChamber.CHAMBER_WIDTH-1)
                    {
                        return false;
                    }

                    if(p.Y + dy <= 0)
                    {
                        return false;
                    }

                    Rock rockAtPoint = ParentChamber.IsThereARockAtPoint(new Point(p.X + dx, p.Y + dy));
                    if (rockAtPoint != null && rockAtPoint.IsInMotion == false)
                    {
                        return false;
                    }
                }

                return true;
            }

            public virtual List<Point> GetAllPoints()
            {
                return new List<Point>();
            }

            public void MoveRockBecauseOfGas(string gasDirection)
            {
                if(gasDirection == "<")
                {
                    if(CanMoveInDirection(-1, 0))
                    {
                        Position.Offset(-1, 0);
                    }
                }
                else
                {
                    if(CanMoveInDirection(1, 0))
                    {
                        Position.Offset(1, 0);
                    }
                }
            }
        }

        class MinusRock : Rock
        {
            public MinusRock(ChamberOfRocks parentChamber) : base(parentChamber)
            {
                this.Position = new Point(3, ParentChamber.PositionOfFloor + 4);
            }

            public override List<Point> GetAllPoints()
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(this.Position.X, this.Position.Y));
                points.Add(new Point(this.Position.X+1, this.Position.Y));
                points.Add(new Point(this.Position.X+2, this.Position.Y));
                points.Add(new Point(this.Position.X+3, this.Position.Y));
                return points;
            }
        }

        class PlusRock : Rock
        {
            public PlusRock(ChamberOfRocks parentChamber) : base(parentChamber)
            {
                this.Position = new Point(3, ParentChamber.PositionOfFloor + 6);
            }

            public override List<Point> GetAllPoints()
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(this.Position.X + 1, this.Position.Y));
                points.Add(new Point(this.Position.X, this.Position.Y - 1));
                points.Add(new Point(this.Position.X + 1, this.Position.Y - 1));
                points.Add(new Point(this.Position.X + 2, this.Position.Y - 1));
                points.Add(new Point(this.Position.X + 1, this.Position.Y - 2));
                return points;
            }
        }

        class BackwardsLShapeRock : Rock
        {
            public BackwardsLShapeRock(ChamberOfRocks parentChamber) : base(parentChamber)
            {
                this.Position = new Point(3, ParentChamber.PositionOfFloor + 6);
            }

            public override List<Point> GetAllPoints()
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(this.Position.X + 2, this.Position.Y));
                points.Add(new Point(this.Position.X + 2, this.Position.Y - 1));
                points.Add(new Point(this.Position.X, this.Position.Y - 2));
                points.Add(new Point(this.Position.X + 1, this.Position.Y - 2));
                points.Add(new Point(this.Position.X + 2, this.Position.Y - 2));
                return points;
            }
        }

        class VertlineRock : Rock
        {
            public VertlineRock(ChamberOfRocks parentChamber) : base(parentChamber)
            {
                this.Position = new Point(3, ParentChamber.PositionOfFloor + 7);
            }

            public override List<Point> GetAllPoints()
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(this.Position.X, this.Position.Y));
                points.Add(new Point(this.Position.X, this.Position.Y - 1));
                points.Add(new Point(this.Position.X, this.Position.Y - 2));
                points.Add(new Point(this.Position.X, this.Position.Y - 3));
                return points;
            }
        }

        class FourClumpRock : Rock
        {
            public FourClumpRock(ChamberOfRocks parentChamber) : base(parentChamber)
            {
                this.Position = new Point(3, ParentChamber.PositionOfFloor + 5);
            }

            public override List<Point> GetAllPoints()
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(this.Position.X, this.Position.Y));
                points.Add(new Point(this.Position.X + 1, this.Position.Y));
                points.Add(new Point(this.Position.X, this.Position.Y - 1));
                points.Add(new Point(this.Position.X + 1, this.Position.Y - 1));
                return points;
            }
        }
    }
}
