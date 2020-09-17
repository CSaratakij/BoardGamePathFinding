using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BoardGame;
using Tree = BoardGame.Tree;

namespace Tests
{
    public class TreeTest
    {
        Tree tree;

        [SetUp]
        public void Initialize()
        {
            tree = new Tree();
        }

        [Test]
        public void Should_AddNodeToParent()
        {
            Node node = new Node(1);
            tree.AddNode(node);

            Assert.Contains(node, tree.Root.children);
            Assert.AreEqual(node.parent, tree.Root);
        }

        [Test]
        public void Should_AddNodeToSpecifyParent()
        {
            Node parent = new Node(1);
            Node child = new Node(2);

            tree.AddNode(parent);
            tree.AddNode(parent, child);

            Assert.Contains(parent, tree.Root.children);
            Assert.AreEqual(parent.parent, tree.Root);

            Assert.Contains(child, parent.children);
            Assert.AreEqual(child.parent, parent);
        }

        [Test]
        public void Should_GetDepth_0_When_StartNodeIsRoot()
        {
            Node root = tree.Root;
            int depth = tree.GetDepth(root);
            Assert.AreEqual(0, depth);
        }

        [Test]
        public void Should_GetDepth_3_When_StartNodeIsDeepUpTo_3()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);

            tree.AddNode(a);
            tree.AddNode(a, b);
            tree.AddNode(b, c);

            int depth = tree.GetDepth(c);
            Assert.AreEqual(3, depth);
        }

        [Test]
        public void Should_GetDepth_Negative_When_ParentNode_IsNull()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);

            tree.AddNode(a);
            tree.AddNode(a, b);
            tree.AddNode(b, c);

            a.parent = null;

            int depth = tree.GetDepth(c);
            Assert.AreEqual(-1, depth);
        }

        [Test]
        public void Should_GetLeafNode_When_NodeHasNoChildren()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);

            tree.AddNode(a);
            tree.AddNode(a, b);
            tree.AddNode(b, c);

            List<Node> leafNodes = tree.GetLeafNodes(tree.Root);

            Assert.Contains(c, leafNodes);
            Assert.AreEqual(1, leafNodes.Count);
        }

        [Test]
        public void Should_GetLeafNode_When_MultipleLeafNode()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);
            Node d = new Node(3);
            Node e = new Node(4);

            tree.AddNode(a);
            tree.AddNode(a, b);
            tree.AddNode(b, c);
            tree.AddNode(b, d);
            tree.AddNode(b, e);

            List<Node> leafNodes = tree.GetLeafNodes(tree.Root);

            CollectionAssert.AreEqual(new Node[] { c, d, e }, leafNodes.ToArray());
            Assert.AreEqual(3, leafNodes.Count);
        }

        [Test]
        public void Should_GetLeafNode_When_MultipleParent_And_MultipleLeafNode()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);
            Node d = new Node(3);
            Node e = new Node(4);
            Node f = new Node(5);

            tree.AddNode(a);
            tree.AddNode(b);

            tree.AddNode(a, c);
            tree.AddNode(a, d);

            tree.AddNode(b, e);
            tree.AddNode(b, f);

            List<Node> leafNodes = tree.GetLeafNodes(tree.Root);

            CollectionAssert.AreEqual(new Node[] { c, d, e, f }, leafNodes.ToArray());
            Assert.AreEqual(4, leafNodes.Count);
        }

        [Test]
        public void Should_GetLeafNode_When_MultipleParent_And_MultipleLeafNode_And_At_DifferentDepth()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);
            Node d = new Node(3);
            Node e = new Node(4);
            Node f = new Node(5);
            Node g = new Node(6);
            Node h = new Node(7);
            Node i = new Node(8);
            Node j = new Node(9);

            tree.AddNode(a);
            tree.AddNode(b);

            tree.AddNode(a, c);
            tree.AddNode(a, d);

            tree.AddNode(b, e);
            tree.AddNode(b, f);

            tree.AddNode(c, j);

            tree.AddNode(e, g);
            tree.AddNode(e, h);
            tree.AddNode(e, i);

            List<Node> leafNodes = tree.GetLeafNodes(tree.Root);

            CollectionAssert.AreEquivalent(new Node[] { j, d, g, h, i, f }, leafNodes.ToArray());
            Assert.AreEqual(6, leafNodes.Count);
        }

        [Test]
        public void Should_GetRoot_When_GetSiblingAt_Depth_0()
        {
            Node a = new Node(0);
            Node b = new Node(1);

            tree.AddNode(a);
            tree.AddNode(b);

            List<Node> siblingNodes = tree.GetSiblingNodes(0);

            CollectionAssert.AreEquivalent(new Node[] { tree.Root }, siblingNodes.ToArray());
            Assert.AreEqual(1, siblingNodes.Count);
        }

        [Test]
        public void Should_GetSibling_When_GetSiblingAt_Depth_1()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(1);

            tree.AddNode(a);
            tree.AddNode(b);
            tree.AddNode(c);

            List<Node> siblingNodes = tree.GetSiblingNodes(1);

            CollectionAssert.AreEquivalent(new Node[] { a, b, c}, siblingNodes.ToArray());
            Assert.AreEqual(3, siblingNodes.Count);
        }

        [Test]
        public void Should_GetSibling_When_GetSiblingAt_Depth_3()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);
            Node d = new Node(3);
            Node e = new Node(4);
            Node f = new Node(5);
            Node g = new Node(6);
            Node h = new Node(7);
            Node i = new Node(8);
            Node j = new Node(9);

            tree.AddNode(a);
            tree.AddNode(b);

            tree.AddNode(a, c);
            tree.AddNode(a, d);

            tree.AddNode(b, e);
            tree.AddNode(b, f);

            tree.AddNode(c, j);

            tree.AddNode(e, g);
            tree.AddNode(e, h);
            tree.AddNode(e, i);

            int depth = 2;
            List<Node> siblingNodes = tree.GetSiblingNodes(depth);

            CollectionAssert.AreEquivalent(new Node[] { c, d, e, f }, siblingNodes.ToArray());
            Assert.AreEqual(4, siblingNodes.Count);
        }

        [Test]
        public void Should_NotGetSibling_When_Tree_IsNot_DeepEnough()
        {
            Node a = new Node(0);
            Node b = new Node(1);
            Node c = new Node(2);
            Node d = new Node(3);
            Node e = new Node(4);
            Node f = new Node(5);
            Node g = new Node(6);
            Node h = new Node(7);
            Node i = new Node(8);
            Node j = new Node(9);

            tree.AddNode(a);
            tree.AddNode(b);

            tree.AddNode(a, c);
            tree.AddNode(a, d);

            tree.AddNode(b, e);
            tree.AddNode(b, f);

            tree.AddNode(c, j);

            tree.AddNode(e, g);
            tree.AddNode(e, h);
            tree.AddNode(e, i);

            int depth = 7;
            List<Node> siblingNodes = tree.GetSiblingNodes(depth);

            CollectionAssert.AreEquivalent(new Node[] { }, siblingNodes.ToArray());
            Assert.AreEqual(0, siblingNodes.Count);
        }
    }
}
