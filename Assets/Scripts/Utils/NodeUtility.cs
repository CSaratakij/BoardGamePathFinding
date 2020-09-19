using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BoardGame
{
    public sealed class NodeUtility
    {
        static readonly Regex EdgeParsePattern = new Regex(@"(^\d+):(.*(?:\d+))");
        static readonly Regex AllNumberParsePattern = new Regex(@"\d+");
        static readonly FormatException formatException = new FormatException("Pattern is not valid");

        public static bool IsPatternValid(string value)
        {
            Match match = EdgeParsePattern.Match(value);
            return match.Success;
        }

        public static ICollection<string> ToStringCollection(Dictionary<int, List<int>> edges)
        {
            ICollection<string> collection = new List<string>();

            foreach (var key in edges.Keys)
            {
                string edgeStringPattern = (key + ":");

                for (int i = 0; i < edges[key].Count; ++i)
                {
                    int value = edges[key][i];
                    string prefix = ((i + 1) == edges[key].Count) ? "" : ",";

                    edgeStringPattern += (value + prefix);
                }

                collection.Add(edgeStringPattern);
            }

            return collection;
        }

        public static Dictionary<int, List<int>> ParseEdge(ICollection<string> value)
        {
            var result = new Dictionary<int, List<int>>();

            foreach (var data in value)
            {
                var dict = ParseEdge(data);

                foreach (var key in dict.Keys)
                {
                    result.Add(key, dict[key]);
                }
            }

            return result;
        }

        public static Dictionary<int, List<int>> ParseEdge(string value)
        {
            value = value.Trim();

            if (!IsPatternValid(value))
            {
                throw formatException;
            }

            var result = new Dictionary<int, List<int>>();
            var mathes = AllNumberParsePattern.Matches(value);

            int key = -1;
            List<int> edges = new List<int>();

            for (int i = 0; i < mathes.Count; ++i)
            {
                try
                {
                    int number = int.Parse(mathes[i].ToString());

                    if (i == 0)
                    {
                        key = number;
                        continue;
                    }

                    edges.Add(number);
                }
                catch (Exception) {}
            }

            result.Add(key, edges);
            return result;
        }
    }
}
