using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Data.SqlTypes;
using System.Security;
using System.Net.Sockets;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<(string List1, string List2)> pairs = new List<(string List1, string List2)>();

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string list1 = sr.ReadLine();
                    string list2 = sr.ReadLine();

                    (string List1, string List2) pair = (list1, list2);
                    pairs.Add(pair);

                    if (!sr.EndOfStream)
                    {
                        sr.ReadLine();
                    }
                }
            }

            ListObj root1 = null;
            ListObj root2 = null;

            int pairNum = 1;

            int goodPairSum = 0;

            List<ListObj> packets = new List<ListObj>();

            foreach ((string List1, string List2) pair in pairs)
            {
                ExtractPairings(ref root1, pair.List1);
                ExtractPairings(ref root2, pair.List2);

                packets.Add(root1);
                packets.Add(root2);

                root1 = null;
                root2 = null;
            }

            int tot = 0;

        

            for (int i = 0; i < packets.Count; i += 2) if (packets[i].CompareTo(packets[i + 1]) < 0) tot += i / 2 + 1;
            Console.WriteLine("Good pair sum=" + tot.ToString());

            int ret = 1;
            List<ListObj> copy = new List<ListObj>(packets);

            List<ListObj> twoList = new List<ListObj>();
            twoList.Add(new ListObj(null, 2, false));
            ListObj two = new ListObj(twoList, -1, true);

            List<ListObj> sixList = new List<ListObj>();
            sixList.Add(new ListObj(null, 6, false));
            ListObj six = new ListObj(sixList, -1, true);

            copy.Add(two);
            copy.Add(six);
            copy.Sort();

            for (int i = 0; i < copy.Count; i++) if (copy[i] == two || copy[i] == six) ret *= i + 1;
            Console.WriteLine("Decoder Key=" + ret.ToString());
        }

        private static void ExtractPairings(ref ListObj root, string s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].ToString() == "[")
                {
                    if (root == null)
                    {
                        root = new ListObj(new List<ListObj>(), -1, isList: true);
                    }
                    else
                    {
                        ListObj lastUnprocessedList = GetLastUnprocessedListObj(root);

                        if (lastUnprocessedList == null)
                        {
                            root.ListInside.Add(new ListObj(new List<ListObj>(), -1, isList: true) { Parent = root });
                        }
                        else
                        {
                            lastUnprocessedList.ListInside.Add(new ListObj(new List<ListObj>(), -1, isList: true) { Parent = lastUnprocessedList });
                        }
                    }
                }
                else if (s[i].ToString() == "]")
                {
                    ListObj lastUnprocessedList = GetLastUnprocessedListObj(root);

                    if (lastUnprocessedList == null) 
                    {
                        Console.WriteLine("TEST");
                    }

                    //set last list to processed
                    lastUnprocessedList.IsListClosed = true;
                }
                else if (s[i].ToString() == ",")
                {

                }
                else
                {
                    int number = -1;
                    string numberString = s[i].ToString();

                    if (int.TryParse(s[i + 1].ToString(),out number))
                    {
                        numberString += s[i + 1].ToString();
                    }

                    ListObj lastUnprocessedList = GetLastUnprocessedListObj(root);
                    lastUnprocessedList.ListInside.Add(new ListObj(null, Convert.ToInt32(numberString), isList: false) { Parent = lastUnprocessedList });

                    if(numberString.Length == 2)
                    {
                        ExtractPairings(ref root, s.Substring(i + 2));
                        break;
                    }
                }

                ExtractPairings(ref root, s.Substring(i + 1));
                break;
            }

        }

        private static ListObj GetLastUnprocessedListObj(ListObj root)
        {
            if (root.IsList && root.ListInside.Any() && !root.IsListClosed && root.ListInside.Any(b => !b.IsListClosed && b.IsList))
            {
                foreach (ListObj list in root.ListInside.Where(b => !b.IsListClosed && b.IsList))
                {
                    return GetLastUnprocessedListObj(list);
                }

                return null;
            }
            else if(root.IsList && !root.IsListClosed)
            {
                return root;
            }
            else
            {
                return null;
            }
        }

        class ListObj : IComparable<ListObj>
        {
            public List<ListObj> ListInside;
            public int IntegerNumber;
            public bool IsList = false;
            public bool IsListClosed = false;
            public ListObj Parent;
            public bool IsInteger
            {
                get
                {
                    return !IsList;
                }
            }

            public ListObj(List<ListObj> listInside, int integerNumber, bool isList)
            {
                ListInside = listInside;
                IntegerNumber = integerNumber;
                IsList = isList;
            }

            public override string ToString()
            {
                string s = "";
                if (IsInteger)
                {
                    return IntegerNumber.ToString();
                }
                else
                {
                    s = "[";

                    s += string.Join(",", ListInside);

                    s += "]";
                }

                return s;
            }

            public int CompareTo(ListObj? other)
            {
                if (ListInside == null && other!.ListInside == null) return IntegerNumber.CompareTo(other.IntegerNumber);
                if (ListInside != null && other!.ListInside != null)
                {
                    int idx = 0;
                    while (idx < ListInside.Count || idx < other.ListInside.Count)
                    {
                        if (idx >= ListInside.Count) return -1;
                        if (idx >= other.ListInside.Count) return 1;
                        int ret = ListInside[idx].CompareTo(other.ListInside[idx]);
                        if (ret != 0) return ret;
                        idx++;
                    }
                    return 0;
                }
                if (other!.ListInside != null) return -other.CompareTo(this);
                if (ListInside != null)
                {
                    if (ListInside.Count == 0) return -1;
                    int ret = ListInside[0].CompareTo(other);
                    if (ret != 0) return ret;
                    if (ListInside.Count > 1) return 1;
                }
                return 0;
            }
        }
    }
}
