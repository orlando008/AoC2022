using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Security;

namespace Day16
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

            Volcano volcano = new Volcano();

            foreach (string s in strings)
            {
                var vals = s.Split(";")[0].Replace("Valve ", "").Replace(" has flow rate", "").Split("=");
                volcano.Valves.Add(new Valve(vals[0], Convert.ToInt32(vals[1])));
            }

            foreach (string s in strings)
            {
                var valveName = s.Split(";")[0].Replace("Valve ", "").Replace(" has flow rate", "").Replace(" ", "").Split("=")[0];

                var vals = s.Split(";")[1].Replace("tunnels", "tunnel").Replace("leads", "lead").Replace("valves", "valve").Replace("tunnel lead to valve ", "").Replace(" ", "").Replace("\r", "").Split(",");

                Valve v = volcano.GetValveWithName(valveName);
                foreach (string subVal in vals)
                {
                    Valve subValve = volcano.GetValveWithName(subVal);
                    v.AddAdjacent(subValve);
                }
            }


            Valve currentValve = volcano.Valves[0];
            Random r = new Random((int)DateTime.Now.Ticks);

            int bestSimulation = 0;

            string simulationString = "";

            List<string> pathsAttempted = new List<string>();

            for (int simulationRound = 0; simulationRound < 500000; simulationRound++)
            {
                int totalPressure = 0;
                currentValve = volcano.Valves[0];

                for (int i = 1; i <= 30; i++)
                {
                    //simulationString += "== Minute " + i + " == \n";

                    if (String.IsNullOrEmpty(volcano.GetListOfValvesOpen()))
                    {
                        //simulationString += "No valves are open\n";
                    }
                    else
                    {
                        totalPressure += volcano.GetCurrentPressureOutFlow();
                        //simulationString += "Valves " + volcano.GetListOfValvesOpen() + " are open, releasing " + volcano.GetCurrentPressureOutFlow().ToString() + " pressure\n";
                    }

                    if (currentValve.Pressure != 0 && !currentValve.IsOpen)
                    {
                        currentValve.IsOpen = true;
                        //simulationString += currentValve.Name + "\n";
                    }
                    else
                    {
                        //move
                        //int valveNum = r.Next(0, currentValve.AdjacentValves.Where(v=>v.Name != simulationString.Substring(simulationString.Count-2)).Count);
                        //currentValve = currentValve.AdjacentValves.Where(v => v.Name != simulationString.Substring(simulationString.Count - 2))[valveNum];
                        simulationString += currentValve.Name;
                       //simulationString += "You move to valve " + currentValve.Name + "\n";
                    }
                }

               // simulationString += "Simulation relieved " + totalPressure.ToString() + "\n";

                if (totalPressure > bestSimulation)
                {
                    bestSimulation = totalPressure;
                }

                //using(StreamWriter sw = new StreamWriter("output.txt", append: true))
                //{
                //    sw.WriteLine(simulationString);
                //}
                pathsAttempted.Add(simulationString);
                simulationString = "";


                foreach (Valve v in volcano.Valves)
                {
                    v.IsOpen = false;
                }
            }

            Console.WriteLine("Best Simulation relieved " + bestSimulation);

        }
    }

    class Volcano
    {
        public List<Valve> Valves = new List<Valve>();

        public Valve GetValveWithName(string name)
        {
            return Valves.FirstOrDefault(v => v.Name == name);
        }

        public override string ToString()
        {
            string s = "----Volcano----\n";

            List<Valve> prevWritten = new List<Valve>();
            int padCount = 0;

            foreach (Valve v in Valves)
            {
                s += v.GetMapString(prevWritten);
                padCount++;
            }

            return s;
        }

        public int GetCurrentPressureOutFlow()
        {
            int outflow = 0;

            foreach (Valve v in Valves)
            {
                if (v.IsOpen)
                {
                    outflow += v.Pressure;
                }
            }

            return outflow;
        }

        public string GetListOfValvesOpen()
        {
            return String.Join(",", Valves.Where(v => v.IsOpen).Select(v => v.Name));
        }
    }

    class Valve
    {
        public string Name;
        public int Pressure;
        public List<Valve> AdjacentValves;
        public bool IsOpen;

        Dictionary<string, int> ValveDistances = new Dictionary<string, int>();

        public Valve(string name, int pressure)
        {
            Name = name;
            Pressure = pressure;
            AdjacentValves = new List<Valve>();
        }

        public void AddAdjacent(Valve valve)
        {
            AdjacentValves.Add(valve);
        }

        public string GetMapString(List<Valve> previouslyWrittenValves)
        {
            string s = "|" + this.Name + "|\n";
            s += "|" + this.Pressure.ToString().PadLeft(2, '0') + "|\n";

            foreach (Valve v in AdjacentValves)
            {
                s += "|  |";
            }

            s += "\n";

            foreach (Valve v in AdjacentValves)
            {
                s += "|" + v.Name + "|";
            }

            s += "\n";

            foreach (Valve v in AdjacentValves)
            {
                s += "|" + v.Pressure.ToString().PadLeft(2, '0') + "|";
            }

            s += "\n";

            return s + "\n" + "\n";
        }
    }
}