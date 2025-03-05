using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Framvik.DotNetLib.Extensions;

public class StringExtensionsTests
{
    [Test]
    public void StringExtensionEndsWithManyTest()
    {
        var stringInvalidEndings = new string[] { "2", " ", ",", "Tum", "#", "CCC", "aaa", "xxx" };
        var stringValidEndings = new string[] { "End", "Pop", "Clam", "Space", "Mort", "Rib", "Fix", "Ver" };
        foreach (var s in stringValidEndings)
        {
            string testString = "A String with many possible endings like " + s;
            // invalid endings should return false
            Assert.IsFalse(testString.EndsWithAny(stringInvalidEndings));
            // any of the randomly used valid should return true
            Assert.IsTrue(testString.EndsWithAny(stringValidEndings));
        }
    }

    [Test]
    public void StringExtensionEndsWithManyCaseInsensitiveTest()
    {
        var stringLowerCaseEndings = new string[] { "end", "pop", "clam", "space", "mort", "rib", "fix", "ver" };
        var stringUpperCaseEndings = System.Array.ConvertAll(stringLowerCaseEndings, (i) => i.ToUpper());

        foreach (var s in stringUpperCaseEndings)
        {
            string testString = "A String with many possible endings like " + s;
            // should return false for lower cases
            Assert.IsFalse(testString.EndsWithAny(stringLowerCaseEndings));
            // should return true for lower cases
            Assert.IsTrue(testString.EndsWithAnyCaseVariants(stringLowerCaseEndings));
        }
    }

    [Test]
    public void StringExtensionReplaceLastTest()
    {
        const string oldValue = "Test";
        const string newValue = "Suffix";
        const string stringTestOld = oldValue + newValue + oldValue + "Pal" + " " + oldValue;
        const string stringTestNew = oldValue + newValue + oldValue + "Pal" + " " + newValue;
        Assert.IsTrue(stringTestOld.ReplaceLast(oldValue, newValue) == stringTestNew);
    }
}
