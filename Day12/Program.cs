using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string totalString;
            List<MapTile> map = new List<MapTile>();

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                int x = 0;
                int y = 0;

                while (!sr.EndOfStream)
                {
                    int newChar = sr.Read();

                    if (newChar == '\n')
                    {
                        y += 1;
                        x = 0;
                    }
                    else
                    {
                        bool isStart = false;
                        bool isEnd = false;

                        if ((char)newChar == 'S')
                        {
                            newChar = (int)'a';
                        }
                        else if ((char)newChar == 'E')
                        {
                            isEnd = true;
                            newChar = (int)'z';
                        }

                        map.Add(new MapTile(x, y, newChar, isStart, isEnd));
                        x += 1;
                    }
                }
            }

            foreach (MapTile mt in map)
            {
                mt.TraversibleTiles = GetTraversibleMapTiles(mt, map);
            }

            List<MapTile> allPossibleStarts = map.Where(m => m.Height == 97).ToList();
            int bestDistance = int.MaxValue;

            foreach (MapTile mapTile in allPossibleStarts)
            {
                int distance = DoMapTest(map, mapTile);

                if (distance < bestDistance)
                    bestDistance = distance;

                foreach(MapTile m in map)
                {
                    m.Distance = int.MaxValue;
                    m.IsProcessed = false;
                }
            }

            Console.WriteLine("Best distance: " + bestDistance.ToString());
        }

        private static int DoMapTest(List<MapTile> map, MapTile startTile)
        {
            MapTile endTile = map.First(m => m.IsEnd);

            List<MapTile> shortestPathSet = new List<MapTile>();
            //Assign a distance value to all vertices in the input graph.
            //Initialize all distance values as INFINITE.
            //Assign the distance value as 0 for the source vertex so that it is picked first. 

            startTile.Distance = 0;

            //While sptSet doesn’t include all vertices
            while (shortestPathSet.Count != map.Count)
            {
                //Pick a vertex u which is not there in sptSet and has a minimum distance value. 
                MapTile mt = map.Where(dd => dd.Distance != int.MaxValue && !dd.IsProcessed)?.OrderBy(m => m.Distance)?.FirstOrDefault();

                if (mt == null)
                    break;

                //Include u to sptSet. 
                shortestPathSet.Add(mt);

                //Then update distance value of all adjacent vertices of u.
                //To update the distance values, iterate through all adjacent vertices. 
                //For every adjacent vertex v,
                //if the sum of the distance value of u (from source) and weight of edge u-v, is less than the distance value of v, then update the distance value of v. 

                foreach (MapTile adjacent in mt.TraversibleTiles)
                {
                    if (mt.Distance < adjacent.Distance)
                    {
                        adjacent.Distance = mt.Distance + 1;
                    }
                }

                mt.IsProcessed = true;

                if (mt.IsEnd)
                {
                    return mt.Distance;
                }
            }

            return int.MaxValue;
        }

        static List<MapTile> GetTraversibleMapTiles(MapTile mt, List<MapTile> map)
        {
            List<MapTile> traversibleTiles = new List<MapTile>();

            MapTile mtUp = map.FirstOrDefault(m => m.X == mt.X && m.Y == mt.Y - 1 && m.Height <= mt.Height + 1);
            MapTile mtDown = map.FirstOrDefault(m => m.X == mt.X && m.Y == mt.Y + 1 && m.Height <= mt.Height + 1);
            MapTile mtLeft = map.FirstOrDefault(m => m.X == mt.X-1 && m.Y == mt.Y && m.Height <= mt.Height + 1);
            MapTile mtRight = map.FirstOrDefault(m => m.X == mt.X+1 && m.Y == mt.Y && m.Height <= mt.Height + 1);

            if(mtUp != null)
            {
                traversibleTiles.Add(mtUp);
            }

            if (mtDown != null)
            {
                traversibleTiles.Add(mtDown);
            }

            if (mtLeft != null)
            {
                traversibleTiles.Add(mtLeft);
            }

            if (mtRight != null)
            {
                traversibleTiles.Add(mtRight);
            }

            return traversibleTiles;
        }

        class MapTile
        {
            public int X;
            public int Y;
            public int Height;
            public bool IsStart;
            public bool IsEnd;
            public List<MapTile> TraversibleTiles;
            public int Distance = int.MaxValue;
            public bool IsProcessed = false;

            public MapTile(int x, int y, int height, bool isStart, bool isEnd)
            {
                X = x;
                Y = y;
                Height = height;
                IsStart = isStart;
                IsEnd = isEnd;
            }

            public override string ToString()
            {
                return (X.ToString() + "," + Y.ToString() + ": Height=" + Height.ToString() + ", Distance=" + Distance.ToString());
            }
        }
    }
}
