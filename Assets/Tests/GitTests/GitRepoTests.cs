using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Framvik.DotNetLib.Git;
using System.IO;
using UnityEditor;

public class GitRepoTests
{
    [Test]
    public void GitRepoParsingTest()
    {
        Assert.IsTrue(GitProcess.SystemHasGitInstalled());

        const string testFolder = "GitRepoParsingTest";
        var path = Path.Combine(FileUtil.GetUniqueTempPathInProject(), testFolder);
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);

        var initProcess = GitProcess.StartAndWaitForExit("init", path);
        Assert.IsTrue(!initProcess.HadErrors, initProcess.ErrorOutput);

        var repo = new GitRepo(path);
        Assert.IsTrue(repo.Valid);

        Directory.Delete(path, true);
    }
}
