using System;
using System.Collections.Generic;

namespace BoardGame
{
    public class Node
    {
        public int id;
        public List<Node> children;
        public Node parent;

        public Node(int id)
        {
            this.id = id;
            this.parent = null;
            this.children = new List<Node>();
        }

        public bool HasParent()
        {
            return (parent != null);
        }

        public override string ToString()
        {
            return string.Format("Node({0})", id);
        }
    }
}
