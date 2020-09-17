using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [Serializable]
    public class Graph
    {
        static readonly ArgumentException ArgumentException = new ArgumentException();
        static readonly KeyNotFoundException KeyNotFoundException = new KeyNotFoundException();
        static readonly ConnectTheSameNodeException ConnectTheSameNodeException = new ConnectTheSameNodeException();

        List<Node> nodes;
        Dictionary<int, List<int>> edges;

        public List<Node> Nodes => nodes;
        public Dictionary<int, List<int>> Edges => edges;

        public Graph()
        {
            nodes = new List<Node>();
            edges = new Dictionary<int, List<int>>();
        }

        public Graph(Node[] nodes) : this()
        {
            AddNode(nodes);
        }

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }

        public void AddNode(Node[] nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public void AddEdge(int from, int to)
        {
            bool isTheSameNode = (from == to);
            bool IsNotValidKey = !(IsNodeValid(from) && IsNodeValid(to));

            if (IsNotValidKey)
                throw KeyNotFoundException;

            if (isTheSameNode)
                throw ConnectTheSameNodeException;

            if (edges.ContainsKey(from))
            {
                edges[from].Add(to);
            }
            else
            {
                edges.Add(from, new List<int>() { to });
            }
        }

        public void RemoveEdge(int from, int to)
        {
            bool IsNotValidKey = !(IsNodeValid(from) && IsNodeValid(to));

            if (IsNotValidKey)
                throw KeyNotFoundException;

            if (edges.ContainsKey(from))
            {
                edges[from].Remove(to);
            }
        }

        public void Clear()
        {
            nodes.Clear();
            ClearEdges();
        }

        public void ClearEdges()
        {
            edges.Clear();
        }

        bool IsNodeValid(int id)
        {
            return (id >= 0) && (id < nodes.Count);
        }

        public Tree ConstructTree(Node node, int maxDepth)
        {
            Node root = new Node(node.id);
            return ConstructTree(root, maxDepth, this);
        }

        Tree ConstructTree(Node node, int maxDepth, Graph graph, int currentDepth = 0, Tree tree = null)
        {
            if (tree == null)
            {
                tree = new Tree(node);
            }

            List<int> childrenID = null;

            if (graph.edges.ContainsKey(node.id))
            {
                childrenID = graph.edges[node.id];
            }

            if (currentDepth < maxDepth)
            {
                for (int i = 0; i < childrenID.Count; ++i)
                {
                    var child = graph.nodes[childrenID[i]];
                    var addNode = new Node(child.id);

                    bool isParentNull = node.parent == null;

                    if (!isParentNull)
                    {
                        if (node.parent.id == addNode.id)
                        {
                            continue;
                        }
                    }

                    tree.AddNode(node, addNode);

                    if (graph.edges.ContainsKey(child.id))
                    {
                        List<int> childrenOfChild = graph.edges[child.id];

                        if (childrenOfChild.Count > 0)
                        {
                            tree = ConstructTree(addNode, maxDepth, graph, currentDepth + 1, tree);
                        }
                    }
                }
            }

            return tree;
        }
    }
}
