using System;
using System.Diagnostics;
using System.Text;

namespace Framvik.DotNetLib.Git
{
    /// <summary>
    /// Acts as a utility container for making git commands inside a System.Process using an invisible window/shell.
    /// </summary>
    public class GitProcess
    {
        private readonly Process m_Process = new();
        private readonly StringBuilder m_OutputBuilder = new();
        private readonly StringBuilder m_ErrorBuilder = new();

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            m_OutputBuilder.AppendLine(e.Data);
            OnOutput?.Invoke(sender, e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            m_OutputBuilder.AppendLine(e.Data);
            m_ErrorBuilder.AppendLine(e.Data);
            OnError?.Invoke(sender, e.Data);
        }

        private ProcessStartInfo GitStartInfo => new()
        {
            Arguments = Arguments,
            CreateNoWindow = true,
            FileName = "git",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            WorkingDirectory = Directory
        };

        private void SetupProcess()
        {
            m_Process.StartInfo = GitStartInfo;
            m_Process.OutputDataReceived += OutputDataReceived;
            m_Process.ErrorDataReceived += ErrorDataReceived;
            m_OutputBuilder.Clear();
            m_ErrorBuilder.Clear();
        }


        /// <summary>
        /// Event for when internal process receives output.
        /// </summary>
        public event EventHandler<string> OnOutput;

        /// <summary>
        /// Event for when internal process receives error.
        /// </summary>
        public event EventHandler<string> OnError;

        /// <summary>
        /// The arguments to use with the git command.
        /// </summary>
        public string Arguments;

        /// <summary>
        /// The working directory for the process.
        /// </summary>
        public string Directory;

        /// <summary>
        /// The output of the process.
        /// </summary>
        public string Output => m_OutputBuilder.ToString();

        /// <summary>
        /// An error output of the process if it has had any.
        /// </summary>
        public string ErrorOutput => m_ErrorBuilder.ToString();

        /// <summary>
        /// Did the process encounter any errors.
        /// </summary>
        public bool HadErrors => !string.IsNullOrWhiteSpace(ErrorOutput);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="directory"></param>
        public GitProcess(string arguments = "-v", string directory = "")
        {
            Arguments = arguments;
            Directory = directory;
        }

        /// <summary>
        /// Starts the underlying process and wait for its exit.
        /// </summary>
        public GitProcess StartAndWaitForExit()
        {
            SetupProcess();

            try
            {
                m_Process.Start();
                m_Process.BeginOutputReadLine();
                m_Process.BeginErrorReadLine();
                m_Process.WaitForExit();
                m_Process.Close();
            }
            catch (Exception e)
            {
                m_OutputBuilder.AppendLine(e.Message);
                m_ErrorBuilder.AppendLine(e.Message);
                OnError?.Invoke(this, e.Message);
                m_OutputBuilder.AppendLine(e.StackTrace);
                m_ErrorBuilder.AppendLine(e.StackTrace);
                OnError?.Invoke(this, e.StackTrace);
            }

            return this;
        }

        /// <summary>
        /// Starts a new underlying git process, wait for its exit and then returns it.
        /// </summary>
        public static GitProcess StartAndWaitForExit(string arguments = "-v", string directory = "")
        {
            var process = new GitProcess(arguments, directory);
            return process.StartAndWaitForExit();
        }

        /// <summary>
        /// Returns true if the running system has git installed in one of its paths.
        /// </summary>
        public static bool SystemHasGitInstalled()
        {
            var process = new GitProcess();
            return process.StartAndWaitForExit().Output.StartsWith("git");
        }
    }
}