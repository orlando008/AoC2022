using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Data.SqlTypes;

namespace Day11
{
    class Program
    {
        public static UInt64 SUPER_MODULUS = 1;

        static void Main(string[] args)
        {
            List<Monkey> monkeys = new List<Monkey>();

            //my input, didn't feel like parsing it off the input txt, so I just typed things in as parameters to the constructor
            monkeys.Add(new Monkey(monkeyNumber:0, 
                startingItems:"71, 86", 
                divisibilityTest:19, 
                monkeyToThrowToIfTrue:6, 
                monkeyToThrowToIfFalse:7, 
                operationForOperation:"*", 
                valueForOperation:13));

            monkeys.Add(new Monkey(monkeyNumber:1, 
                startingItems: "66, 50, 90, 53, 88, 85", 
                divisibilityTest: 2, 
                monkeyToThrowToIfTrue: 5, 
                monkeyToThrowToIfFalse: 4, 
                operationForOperation: "+", 
                valueForOperation: 3));

            monkeys.Add(new Monkey(monkeyNumber:2, 
                startingItems:"97, 54, 89, 62, 84, 80, 63", 
                divisibilityTest: 13, 
                monkeyToThrowToIfTrue: 4, 
                monkeyToThrowToIfFalse: 1, 
                operationForOperation: "+", 
                valueForOperation: 6));

            monkeys.Add(new Monkey(monkeyNumber:3, 
                startingItems:"82, 97, 56, 92", 
                divisibilityTest: 5, 
                monkeyToThrowToIfTrue: 6, 
                monkeyToThrowToIfFalse: 0, 
                operationForOperation: "+", 
                valueForOperation: 2));

            monkeys.Add(new Monkey(monkeyNumber:4, 
                startingItems:"50, 99, 67, 61, 86", 
                divisibilityTest: 7, 
                monkeyToThrowToIfTrue: 5, 
                monkeyToThrowToIfFalse: 3, 
                operationForOperation: "*", 
                valueForOperation: 0));

            monkeys.Add(new Monkey(monkeyNumber:5, 
                startingItems:"61, 66, 72, 55, 64, 53, 72, 63", 
                divisibilityTest: 11, 
                monkeyToThrowToIfTrue: 3, 
                monkeyToThrowToIfFalse: 0, 
                operationForOperation: "+", 
                valueForOperation: 4));

            monkeys.Add(new Monkey(monkeyNumber:6, 
                startingItems:"59, 79, 63", 
                divisibilityTest: 17, 
                monkeyToThrowToIfTrue: 2, 
                monkeyToThrowToIfFalse: 7, 
                operationForOperation: "*", 
                valueForOperation: 7));

            monkeys.Add(new Monkey(monkeyNumber:7, 
                startingItems:"55", 
                divisibilityTest: 3, 
                monkeyToThrowToIfTrue: 2,
                monkeyToThrowToIfFalse: 1, 
                operationForOperation: "+", 
                valueForOperation: 7));

            ////test input
            //monkeys.Add(new Monkey(0, "79, 98", 23, 2, 3, "*", 19));
            //monkeys.Add(new Monkey(1, "54, 65, 75, 74", 19, 2, 0, "+", 6));
            //monkeys.Add(new Monkey(2, "79, 60, 97", 13, 1, 3, "*", -1));
            //monkeys.Add(new Monkey(3, "74", 17, 0, 1, "+", 3));


            foreach (Monkey m in monkeys)
            {
                LogMessage(m.StarterString());
                //Multiply every monkey's divisible test by each other to get a really big number.  This number becomes the possible universe of numbers, everything gets modulus'd by it.
                //I'm not going to pretend like I totally get it, but it reminds me of changing the speed of light, in a very general sense I get the concept but I don't understand fully.
                SUPER_MODULUS *= m.DivisibilityTest;
            }

            //Lots of LogMessage commented out to speed up the execution.  The logs help with troubleshooting though.

            for (int i = 0; i < 10000; i++)
            {
                foreach (Monkey m in monkeys)
                {
                    //LogMessage($"Monkey {m.MonkeyNumber}'s turn.");

                    for (int itemIndex = 0; itemIndex < m.MonkeyItems.Count; itemIndex++)
                    {
                        //LogMessage($">>>>Inspects item with worry level {m.MonkeyItems[itemIndex]}.");
                        m.MonkeyItems[itemIndex] = m.NewWorry(m.MonkeyItems[itemIndex]);
                        //LogMessage($"Worry level transforms [{m.TransformFormula()}] to {m.MonkeyItems[itemIndex]}.");

                        //After Part 1, no more divide by 3 relief!
                        //m.MonkeyItems[itemIndex] = (long)Math.Floor((decimal)m.MonkeyItems[itemIndex] / (decimal)3);
                        //LogMessage($"Monkey gets bored with item. Worry level is divided by 3 to {m.MonkeyItems[itemIndex]}.");

                        //Magical modulus math for worry relief, Dr. Eric Wastl prescribes 9699690 Zolofts per item and things will be okay (well, not okay but "manageable").
                        m.MonkeyItems[itemIndex] %= SUPER_MODULUS;

                        if (m.MonkeyItems[itemIndex] % m.DivisibilityTest == 0)
                        {
                            //LogMessage($"Current worry level is divisible by {m.DivisibilityTest}");
                            //LogMessage($"Item with worry level {m.MonkeyItems[itemIndex]} is thrown to monkey {m.MonkeyToThrowToIfTrue}.");
                            monkeys[m.MonkeyToThrowToIfTrue].MonkeyItems.Add(m.MonkeyItems[itemIndex]);
                        }
                        else
                        {
                            //LogMessage($"Current worry level is NOT divisible by {m.DivisibilityTest}");
                            //LogMessage($"Item with worry level {m.MonkeyItems[itemIndex]} is thrown to monkey {m.MonkeyToThrowToIfFalse}.");
                            monkeys[m.MonkeyToThrowToIfFalse].MonkeyItems.Add(m.MonkeyItems[itemIndex]);
                        }

                        if (m.MonkeyItems[itemIndex] < 0)
                        {
                            break;
                        }
                    }

                    //LogMessage($"Total Inspections for Monkey {m.MonkeyNumber} = {m.TotalInspections}");

                    m.MonkeyItems.Clear();
                    //LogMessage("");
                    //LogMessage("");
                }

                //LogMessage($"After round {i + 1}, the monkeys are holding items with these worry levels:");
                //foreach (Monkey m in monkeys)
                //{
                //    LogMessage(m.ToString());
                //}
            }

            LogMessage("TOP 2 INSPECTORS:");

            LogMessage((monkeys.OrderByDescending(m => m.TotalInspections).Take(2)).ToArray()[0]);
            LogMessage((monkeys.OrderByDescending(m => m.TotalInspections).Take(2)).ToArray()[1]);

            LogMessage("Total Monkey Business:");
            LogMessage((monkeys.OrderByDescending(m => m.TotalInspections).Take(2)).ToArray()[0].TotalInspections * (monkeys.OrderByDescending(m => m.TotalInspections).Take(2)).ToArray()[1].TotalInspections);
        }

        static void LogMessage(dynamic msg)
        {
            Console.WriteLine(msg);
        }

        class Monkey
        {
            public int MonkeyNumber;
            public List<UInt64> MonkeyItems;

            public UInt64 DivisibilityTest;
            public int MonkeyToThrowToIfTrue;
            public int MonkeyToThrowToIfFalse;
            public string OperationForOperation;
            public UInt64 ValueForOperation;

            public string StartingItems;

            public UInt64 TotalInspections = 0;

            public Monkey(int monkeyNumber, string startingItems, UInt64 divisibilityTest, int monkeyToThrowToIfTrue, int monkeyToThrowToIfFalse, string operationForOperation, UInt64 valueForOperation)
            {
                MonkeyNumber = monkeyNumber;
                MonkeyItems = new List<UInt64>();
                StartingItems = startingItems;
                foreach (string itemWorry in startingItems.Replace(" ", "").Split(",").ToList())
                {
                    MonkeyItems.Add(Convert.ToUInt64(itemWorry));
                }
                DivisibilityTest = divisibilityTest;
                MonkeyToThrowToIfTrue = monkeyToThrowToIfTrue;
                MonkeyToThrowToIfFalse = monkeyToThrowToIfFalse;
                OperationForOperation = operationForOperation;
                ValueForOperation = valueForOperation;
            }

            public UInt64 NewWorry(UInt64 oldWorry)
            {

                TotalInspections += 1;

                UInt64 valueForOperationTemp = this.ValueForOperation;

                if (ValueForOperation == (UInt64)0)
                    valueForOperationTemp = oldWorry;

                switch (OperationForOperation)
                {
                    case "+":
                        return oldWorry + valueForOperationTemp;
                    case "*":
                        return oldWorry * valueForOperationTemp;
                    default:
                        return oldWorry;
                }
            }

            public string TransformFormula()
            {
                switch (OperationForOperation)
                {
                    case "+":
                        return "old + " + (ValueForOperation == 0 ? "old" : ValueForOperation.ToString());
                    case "*":
                        return "old * " + (ValueForOperation == 0 ? "old" : ValueForOperation.ToString());
                    default:
                        return "";
                }
            }

            public override string ToString()
            {
                string s = "Monkey " + this.MonkeyNumber.ToString() + ": ";

                if (MonkeyItems.Any())
                {
                    s += string.Join(", ", MonkeyItems);
                }
                else
                {
                    s += "NO ITEMS PRESENT";
                }

                s += "  |  Total Inspections up to this point: " + TotalInspections.ToString();

                return s;
            }

            public string StarterString()
            {
                string s = $"Monkey {this.MonkeyNumber}:\r\n  Starting items: {StartingItems}\r\n  Operation: new = {TransformFormula()}\r\n  Test: divisible by {DivisibilityTest}\r\n    If true: throw to monkey {MonkeyToThrowToIfTrue}\r\n    If false: throw to monkey {MonkeyToThrowToIfFalse}";
                return s;
            }
        }
    }
}
