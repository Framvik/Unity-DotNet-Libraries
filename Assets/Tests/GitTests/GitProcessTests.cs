using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Framvik.DotNetLib.Git;

public class GitProcessTests
{
    [Test]
    public void GitProcessIsInstalledTest()
    {
        Assert.IsTrue(GitProcess.SystemHasGitInstalled());
    }
}
