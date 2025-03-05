using System.IO;

namespace Framvik.DotNetLib.Git
{
    /// <summary>
    /// Attempts to read a git repository from given path and parse simple information from it.
    /// </summary>
    public class GitRepo
    {
        /// <summary>
        /// The path was a valid git repository.
        /// </summary>
        public bool Valid { get; private set; }

        /// <summary>
        /// The git repository existed online.
        /// </summary>
        public bool Online { get; private set; }

        /// <summary>
        ///  The path to the .git folder.
        /// </summary>
        public string GitPath { get; private set; }

        /// <summary>
        /// The path to the config file inside the .git folder.
        /// </summary>
        public string ConfigPath { get; private set; }

        /// <summary>
        /// The remote origin Url defined inside the config file.
        /// </summary>
        public string ConfigRemoteOriginUrl { get; private set; }

        /// <summary>
        /// The path to the description file inside the .git folder.
        /// </summary>
        public string DescriptionPath { get; private set; }

        /// <summary>
        /// The text inside the description file.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Attempts to parse information about a git repository at given path.
        /// </summary>
        public GitRepo(string path) 
        {
            if (!Directory.Exists(path))
                return;
            GitPath = Path.Combine(path, ".git");
            if (!Directory.Exists(GitPath))
                return;
            ConfigPath = Path.Combine(GitPath, "config");
            if (!File.Exists(ConfigPath))
                return;
            DescriptionPath = Path.Combine(GitPath, "description");
            if (!File.Exists(DescriptionPath))
                return;
            Description = File.ReadAllText(DescriptionPath);
            var config = File.ReadAllLines(ConfigPath);
            var underRemote = false;
            foreach (var line in config)
            {
                if (line == "[remote \"origin\"]")
                    underRemote = true;
                if (underRemote)
                {
                    var parts = line.Trim().Replace(" ", "").Split('=');
                    if (parts.Length > 1 && parts[0].ToLower() == "url")
                    {
                        ConfigRemoteOriginUrl = parts[1];
                        break;
                    }
                }
            }
            Valid = true;
            if (string.IsNullOrEmpty(ConfigRemoteOriginUrl))
                return;
            var lsRemote = new GitProcess("ls-remote -h \"" + ConfigRemoteOriginUrl + "\" &> /dev/null", path);
            lsRemote.StartAndWaitForExit();
            Online = !lsRemote.HadErrors;
        }
    }
}