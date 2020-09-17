using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BoardGame;

namespace Tests
{
    public class NodeTest
    {
        [Test]
        public void Should_NotHaveParent_When_ParentIsNull()
        {
            Node node = new Node(0);
            Assert.IsFalse(node.HasParent());
        }

        [Test]
        public void Should_HaveParent_When_ParentIsExist()
        {
            Node parent = new Node(0);
            Node node = new Node(1);

            node.parent = parent;
            Assert.IsTrue(node.HasParent());
        }
    }
}
