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

            var strings = totalString.Split("\n");

            List<SignalBeacon> beaconList = new List<SignalBeacon>();

            foreach(string s in strings ) 
            {
                var sensorString = s.Split(":")[0].Trim();
                var beaconString = s.Split(":")[1].Trim();

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

            int maxX = beaconList.Max(b => b.SensorX);
            if (beaconList.Max(b => b.BeaconX) > maxX)
            {
                maxX = beaconList.Max(b => b.BeaconX);
            }

            int minX = beaconList.Min(b => b.SensorX);
            if (beaconList.Min(b => b.BeaconX) < minX)
            {
                minX = beaconList.Min(b => b.BeaconX);
            }

            int badPointCount = 0;
            for (int x = minX; x < maxX; x++)
            {
                SignalBeacon nearestBeacon = null;
                int nearestDistSoFar = int.MaxValue;

                if(beaconList.Any(p=>p.SensorX == x && p.SensorY == 2000000))
                {
                    continue;
                }
                else if (beaconList.Any(p => p.BeaconX == x && p.BeaconY == 2000000))
                {
                    continue;
                }

                foreach (SignalBeacon beacon in beaconList)
                {
                    int mhd = SignalBeacon.GetManhattanDistance(x, 2000000, beacon.SensorX, beacon.SensorY);
                    if(mhd < beacon.ManhattanDistance)
                    {
                        badPointCount += 1;
                        Console.WriteLine("Bad point found at " + x);
                        break;
                    }
                }             
            }

            Console.WriteLine("Bad points found in row 2000000=" + badPointCount.ToString());
        }

        class SignalBeacon
        {
            public int SensorX;
            public int SensorY;

            public int BeaconX;
            public int BeaconY;

            public SignalBeacon(int sensorX, int sensorY, int beaconX, int beaconY)
            {
                SensorX = sensorX;
                SensorY = sensorY;
                BeaconX = beaconX;
                BeaconY = beaconY;
            }

            public int ManhattanDistance
            {
                get
                {
                    return GetManhattanDistance(SensorX, SensorY, BeaconX, BeaconY);
                }
            }

            public override string ToString()
            {
                return $"Sensor @ ({SensorX},{SensorY}) Beacon @ ({BeaconX},{BeaconY}, Manhattan Dist = {ManhattanDistance})";
            }

            public static int GetManhattanDistance(int x1, int y1, int x2, int y2)
            {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }
        }
    }


}
