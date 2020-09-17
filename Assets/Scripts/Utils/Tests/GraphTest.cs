using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BoardGame;
using Tree = BoardGame.Tree;

namespace Tests
{
    public class GraphTest
    {
        Graph graph;
        Node[] nodes;

        [SetUp]
        public void Initialize()
        {
            nodes = new Node[] {
                new Node(0),
                new Node(1),
                new Node(2),
                new Node(3),
                new Node(4),
                new Node(5),
                new Node(6)
            };

            graph = new Graph(nodes);
        }

        [Test, Pairwise]
        public void Should_ConnectEdge_When_AddEdge([Values(0, 2)] int from, [Values(1, 3)] int to)
        {
            Assert.DoesNotThrow(() => {
                graph.AddEdge(from, to);
                Assert.Contains(to, graph.Edges[from]);
            });
        }

        [Test, Combinatorial]
        public void Shoud_NotConnectEdge_When_NodeNotExist([Values(-1, -2)] int from, [Values(1, 3, 4)] int to)
        {
            Assert.Throws<KeyNotFoundException>(() => {
                graph.AddEdge(from, to);
            });
        }

        [Test, Sequential]
        public void Shoud_NotConnectEdge_When_ConnectTheSameNode([Values(1, 2, 3)] int from, [Values(1, 2, 3)] int to)
        {
            Assert.Throws<ConnectTheSameNodeException>(() => {
                graph.AddEdge(from, to);
            });
        }

        [Test]
        public void Should_ConstructTree_When_Given_Depth_0()
        {
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 3);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 5);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 2);
            graph.AddEdge(5, 6);

            Tree tree = graph.ConstructTree(graph.Nodes[0], 0);
            List<Node> leafNodes = tree.GetLeafNodes();

            Assert.AreEqual(graph.Nodes[0].id, tree.Root.id);
            Assert.AreEqual(1, leafNodes.Count);
            Assert.AreEqual(graph.Nodes[0].id, leafNodes[0].id);
        }

        [Test]
        public void Should_ConstructTree_When_Given_Depth_4()
        {
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 3);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 5);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 2);
            graph.AddEdge(5, 6);

            Tree tree = graph.ConstructTree(graph.Nodes[0], 4);
            List<Node> leafNodes = tree.GetLeafNodes();

            Assert.AreEqual(graph.Nodes[0].id, tree.Root.id);
            Assert.AreEqual(2, leafNodes.Count);
            Assert.AreEqual(graph.Nodes[6].id, leafNodes[0].id);
            Assert.AreEqual(graph.Nodes[5].id, leafNodes[1].id);
        }

        [Test]
        public void Should_ConstructTree_When_Edges_IsBackAndFourth()
        {
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 0);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 4);
            graph.AddEdge(2, 0);

            Tree tree = graph.ConstructTree(graph.Nodes[0], 2);
            List<Node> leafNodes = tree.GetLeafNodes();

            Assert.AreEqual(graph.Nodes[0].id, tree.Root.id);
            Assert.AreEqual(2, leafNodes.Count);
            Assert.AreEqual(graph.Nodes[4].id, leafNodes[0].id);
            Assert.AreEqual(graph.Nodes[4].id, leafNodes[1].id);
        }
    }
}
