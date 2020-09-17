using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Tree
    {
        static readonly ArgumentOutOfRangeException argumentOutOfRangeException = new ArgumentOutOfRangeException();

        Node root = null;
        public Node Root => root;

        public Tree()
        {
            root = new Node(0);
        }

        public Tree(Node root) : this()
        {
            this.root = root;
        }

        public void Clear()
        {
            root.children.Clear();
        }

        public void AddNode(Node node)
        {
            node.parent = root;
            root.children.Add(node);
        }

        public void AddNode(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public void AddNode(Node parent, Node node)
        {
            node.parent = parent;
            parent.children.Add(node);
        }

        public void AddNode(Node parent, List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(parent, nodes);
            }
        }

        public int GetDepth(Node node, int depth = 0)
        {
            if (root == node)
            {
                return depth;
            }
            else
            {
                if (node.parent == null)
                {
                    return -1;
                }

                return GetDepth(node.parent, depth + 1);
            }
        }

        public List<Node> GetLeafNodes(Node startNode = null)
        {
            if (startNode == null)
            {
                startNode = root;
            }

            return GetLeafNodeRecursive(startNode);
        }

        List<Node> GetLeafNodeRecursive(Node startNode, List<Node> list = null)
        {
            if (list == null)
            {
                list = new List<Node>();
            }

            if (startNode.children.Count > 0)
            {
                foreach (var node in startNode.children)
                {
                    list = GetLeafNodeRecursive(node, list);
                }

                return list;
            }
            else
            {
                list.Add(startNode);
            }

            return list;
        }

        public List<Node> GetSiblingNodes(int depth)
        {
            return GetSiblingNodesRecursive(depth);
        }

        List<Node> GetSiblingNodesRecursive(int maxDepth, int currentDepth = 0, Node startNode = null, List<Node> list = null)
        {
            if (list == null)
            {
                startNode = root;
                list = new List<Node>();
            }

            if (currentDepth == maxDepth)
            {
                list.Add(startNode);
                return list;
            }
            else
            {
                foreach (var node in startNode.children)
                {
                    list = GetSiblingNodesRecursive(maxDepth, currentDepth + 1, node, list);
                }

                return list;
            }
        }
    }
}
