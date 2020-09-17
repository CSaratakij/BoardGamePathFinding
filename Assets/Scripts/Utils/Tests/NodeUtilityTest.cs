using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BoardGame;

namespace Tests
{
    public class NodeUtilityTest
    {
        Dictionary<int, List<int>> result;

        [Test, Sequential]
        public void Should_BeTrue_When_ParseEdgePatternIsValid([Values("0:1, 2, 3", "1:,3,,5", "1:1,2", "2:0,3", "6:1,2,4,5,")] string data)
        {
            Assert.IsTrue(NodeUtility.IsPatternValid(data));
        }

        [Test, Sequential]
        public void Should_BeFalse_When_ParseEdgePatternIsNotValid([Values("01,2,3", ",1:1,2", "", ":1,2,4,5,")] string data)
        {
            Assert.IsFalse(NodeUtility.IsPatternValid(data));
        }

        [Test, Sequential]
        public void Should_ParseString_When_FormatIsCorrect([Values("0:1,2,3", "1:1,2", "2:0,3")] string data)
        {
            Assert.DoesNotThrow(() => {
                result = NodeUtility.ParseEdge(data);
            });
        }

        [Test, Sequential]
        public void Should_NotParseString_When_FormatIsNotValid([Values("01,2,3", ",1:1,2", "", ":1,2,4,5,")] string data)
        {
            Assert.Throws<FormatException>(() => {
                result = NodeUtility.ParseEdge(data);
            });
        }

        [Test]
        public void Should_ParseString_When_MultipleEdgeFormatPattern()
        {
            var data = new string[] {
                "0:1,2,3",
                "2:3,4:5",
                "6:7,8"
            };

            var result = NodeUtility.ParseEdge(data);

            CollectionAssert.IsSubsetOf(new int[]{ 1, 2, 3}, result[0].ToArray());
            CollectionAssert.IsSubsetOf(new int[]{ 3, 4, 5}, result[2].ToArray());
            CollectionAssert.IsSubsetOf(new int[]{ 7, 8 }, result[6].ToArray());
        }
    }
}
