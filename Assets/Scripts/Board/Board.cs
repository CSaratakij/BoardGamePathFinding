using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Board
    {
        static readonly ArgumentException argumentException = new ArgumentException();

        Graph graph;
        Dictionary<int, List<int>[]> possiblePath;

        public Graph Graph => graph;

        public Board()
        {
            graph = new Graph();
            possiblePath = new Dictionary<int, List<int>[]>();
        }

        public Board(Graph graph)
        {
            this.graph = graph;
            possiblePath = new Dictionary<int, List<int>[]>();
        }

        Dictionary<int, List<List<int>>> RemoveUnavailableDestination(Dictionary<int, List<List<int>>> path)
        {
            int[] keys = new int[path.Keys.Count];
            path.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; ++i) {
                var key = keys[i];

                if (path[key].Count == 0) {
                    path.Remove(key);
                }
            }

            return path;
        }

        public Dictionary<int, List<List<int>>> GetPossiblePath(Node node, int maxDepth)
        {
            var paths = new Dictionary<int, List<List<int>>>();

            var treePath = graph.ConstructTree(node, maxDepth);
            var leafNodes = treePath.GetLeafNodes();

            for (int i = 0; i < leafNodes.Count; ++i)
            {
                int id = leafNodes[i].id;

                if (!paths.ContainsKey(id))
                {
                    paths.Add(id, new List<List<int>>());
                }

                var currentPath = new List<int>();
                var startNode = leafNodes[i];

                while (startNode.parent != null)
                {
                    currentPath.Add(startNode.id);
                    startNode = startNode.parent;
                }

                if (currentPath.Count < maxDepth)
                {
                    continue;
                }

                currentPath.Add(treePath.Root.id);
                currentPath.Reverse();

                paths[id].Add(currentPath);
            }

            paths = RemoveUnavailableDestination(paths);
            return paths;
        }
    }
}
