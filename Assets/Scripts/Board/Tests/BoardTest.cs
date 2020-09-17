using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BoardGame;

namespace Tests
{
    public class BoardTest
    {
        Graph graph;
        Board boardGamePath;
        Dictionary<int, List<List<int>>> paths;

        [OneTimeSetUp]
        public void Initialize()
        {
            Node[] nodes = new Node[] {
                new Node(0),
                new Node(1),
                new Node(2),
                new Node(3),
                new Node(4),
                new Node(5),
                new Node(6)
            };

            graph = new Graph(nodes);

            graph.AddEdge(0, 1);
            graph.AddEdge(0, 3);

            graph.AddEdge(1, 0);
            graph.AddEdge(1, 2);

            graph.AddEdge(2, 1);
            graph.AddEdge(2, 3);
            graph.AddEdge(2, 4);

            graph.AddEdge(3, 0);
            graph.AddEdge(3, 2);
            graph.AddEdge(3, 6);

            graph.AddEdge(4, 2);
            graph.AddEdge(4, 5);

            graph.AddEdge(5, 4);
            graph.AddEdge(5, 6);

            graph.AddEdge(6, 3);
            graph.AddEdge(6, 5);

            boardGamePath = new Board(graph);
        }

        [Test]
        public void Should_GetPossiblePath_When_AskForPath()
        {
            paths = boardGamePath.GetPossiblePath(graph.Nodes[0], 4);

            Assert.Contains(0, paths.Keys);
            Assert.Contains(6, paths.Keys);
            Assert.Contains(5, paths.Keys);
            Assert.Contains(4, paths.Keys);

            Assert.AreEqual(2, paths[0].Count);
            Assert.AreEqual(1, paths[6].Count);
            Assert.AreEqual(2, paths[5].Count);
            Assert.AreEqual(1, paths[4].Count);

            CollectionAssert.AreEqual(new int[] {
                0, 1, 2, 3, 0
            }, paths[0][0]);

            CollectionAssert.AreEqual(new int[] {
                0, 3, 2, 1, 0
            }, paths[0][1]);

            CollectionAssert.AreEqual(new int[] {
                0, 1, 2, 3, 6
            }, paths[6][0]);

            CollectionAssert.AreEqual(new int[] {
                0, 1, 2, 4, 5
            }, paths[5][0]);

            CollectionAssert.AreEqual(new int[] {
                0, 3, 2, 4, 5
            }, paths[5][1]);

            CollectionAssert.AreEqual(new int[] {
                0, 3, 6, 5, 4
            }, paths[4][0]);
        }
    }
}
