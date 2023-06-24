// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.Text;
using Reko.Core.Hll.Pascal;

namespace Mosa.Utility.Launcher
{
	public class ExternalProcess
	{
		#region Properties

		public string Output
		{ get { lock (OutputData) { return OutputData.ToString(); } } }

		public string Error
		{ get { lock (ErrorData) { return ErrorData.ToString(); } } }

		public bool HasExited => Process.HasExited;

		public int ExitCode => Process.ExitCode;

		public Process Process = new Process();

		#endregion Properties

		protected ProcessStartInfo ProcessStartInfo => Process.StartInfo;

		protected StringBuilder OutputData = new StringBuilder();

		protected StringBuilder ErrorData = new StringBuilder();

		public ExternalProcess(string app, string args, bool captureOutput = true)
		{
			ProcessStartInfo.FileName = app;
			ProcessStartInfo.Arguments = args;

			ProcessStartInfo.RedirectStandardOutput = captureOutput;
			ProcessStartInfo.RedirectStandardError = captureOutput;
			ProcessStartInfo.UseShellExecute = false;
			ProcessStartInfo.CreateNoWindow = !captureOutput;

			if (captureOutput)
			{
				Process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
				{
					lock (OutputData)
					{
						OutputData.Append(e.Data);
					}
				});

				Process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
				{
					lock (ErrorData)
					{
						ErrorData.Append(e.Data);
					}
				});
			}
		}

		public void Start()
		{
			Process.Start();

			if (ProcessStartInfo.RedirectStandardOutput)
			{
				Process.BeginOutputReadLine();
				Process.BeginErrorReadLine();
			}
		}

		public void Kill()
		{
			Process.Kill();
		}

		public void WaitForExit()
		{
			Process.WaitForExit();
		}

		public bool WaitForExit(int milliseconds)
		{
			return Process.WaitForExit(milliseconds);
		}
	}
}
