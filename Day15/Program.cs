using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string totalString;
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                totalString = sr.ReadToEnd();
            }

            int rowToCheck = 2000000;

            var strings = totalString.Split("\n");

            List<SignalBeacon> beaconList = new List<SignalBeacon>();


            foreach (string s in strings) 
            {
                var sensorString = s.Replace("Sensor at x=", "").Replace("closest beacon is at x=", "").Replace("y=", "").Split(":")[0].Trim();
                var beaconString = s.Replace("Sensor at x=", "").Replace("closest beacon is at x=", "").Replace("y=", "").Split(":")[1].Trim();

                beaconList.Add(new SignalBeacon(
                    Convert.ToInt32(sensorString.Split(",")[0].Trim()), 
                    Convert.ToInt32(sensorString.Split(",")[1].Trim()), 
                    Convert.ToInt32(beaconString.Split(",")[0].Trim()), 
                    Convert.ToInt32(beaconString.Split(",")[1].Trim())));

            }

            foreach(SignalBeacon beacon in beaconList )
            {
                Console.WriteLine(beacon.ToString());
            }

            long otherCount = CountIneligibleBeaconPositions(beaconList);
            Console.WriteLine("Bad points found in row =" + otherCount.ToString());

            Solve2(beaconList, 4000000);
        }

        static long CountIneligibleBeaconPositions(List<SignalBeacon> listSigs)
        {
            const int ROW_CONST = 2_000_000;

            var sensorReport = new Dictionary<Point, Point>();
            var signals = new HashSet<Point>();

            foreach (var item in listSigs)
            {
                // Find the distance between the beacons and sensors x coordinates.
                var xDistance = item.SensorX > item.BeaconX ?
                    item.SensorX - item.BeaconX :
                    item.BeaconX - item.SensorX;

                // Find the distance between the beacons and sensors y coordinates.
                var yDistance = item.SensorY > item.BeaconY ?
                    item.SensorY - item.BeaconY :
                    item.BeaconY - item.SensorY;

                // Find the maximum radius of the sensor's signal area for its closest beacon.
                var signalRadius = xDistance + yDistance;

                // If y = 2_000_000 is not inside the signal area, continue.
                if (ROW_CONST > signalRadius + item.SensorY &&
                    ROW_CONST < item.SensorY - signalRadius)
                    continue;

                // Find the distance between y = 2_000_000 and the signal area radius.
                var rowDistance = ROW_CONST > item.SensorY ?
                    (item.SensorY + signalRadius) - ROW_CONST :
                    ROW_CONST - (item.SensorY - signalRadius);

                // Loop through every x coordinate for point that is inside the signal area, and has y = 2_000_000.
                for (var x = item.SensorX - rowDistance; x < item.SensorX + rowDistance; x++)
                    signals.Add(new Point(x, ROW_CONST));
            }

            return signals.Count;
        }

        static void Solve2(List<SignalBeacon> sensors, int maxBound)
        {
            for (var y = 0; y <= maxBound; y++)
            {
                //Get the sensor points relative to the total frame bounding blocks
                List<int[]> bounds = sensors.Select(s => new int[] { Math.Max(s.MinXAtY(y), 0), Math.Min(s.MaxXAtY(y), maxBound) })
                    .Where(e => e[0] <= e[1]).ToList();

                //Sort bounding block points
                bounds.Sort((a, b) => a[0].CompareTo(b[0]));

                var isMerged = true;

                while (isMerged && bounds.Count > 1)
                {
                    isMerged = false;

                    //if the point at the 1st X min is less than or equal to the 2nd X min
                    //and the point at the 1st X max is greater than or equal to the 2nd X max
                    if (bounds[0][0] <= bounds[1][0] && bounds[0][1] >= bounds[1][0])
                    {
                        //move data 
                        bounds[0][1] = Math.Max(bounds[0][1], bounds[1][1]);

                        //remove the current point from consideration
                        bounds.RemoveAt(1);

                        //continue the while
                        isMerged = true;
                    }
                }

                if (!isMerged || bounds[0][0] != 0 || bounds[0][1] != maxBound)
                {
                    Console.WriteLine("Signal=" + ((Int64)(bounds[0][1] + 1)) * maxBound + y);
                    break;
                }
            }
        }

        class SignalBeacon
        {
            public int SensorX;
            public int SensorY;

            public int BeaconX;
            public int BeaconY;

            public int ManhattanDistance;

            public SignalBeacon(int sensorX, int sensorY, int beaconX, int beaconY)
            {
                SensorX = sensorX;
                SensorY = sensorY;
                BeaconX = beaconX;
                BeaconY = beaconY;
                this.ManhattanDistance = GetManhattanDistance(SensorX, SensorY, BeaconX, BeaconY);
            }

            public override string ToString()
            {
                return $"Sensor @ ({SensorX},{SensorY}) Beacon @ ({BeaconX},{BeaconY}, Manhattan Dist = {ManhattanDistance})";
            }

            public static int GetManhattanDistance(int x1, int y1, int x2, int y2)
            {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }

            public int MinXAtY(int y) => SensorX - ManhattanDistance + Math.Abs(SensorY - y);
            public int MaxXAtY(int y) => SensorX + ManhattanDistance - Math.Abs(SensorY - y);
        }
    }


}
