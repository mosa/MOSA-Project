/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;
using System;
using System.Threading;

namespace Mosa.Utility.Launcher
{
	internal class BuilderEventListener : ICompilerEventListener
	{
		private Builder builder;
		private object mylock = new object();

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			lock (mylock)
			{
				if (compilerStage == CompilerEvent.CompilerStageStart || compilerStage == CompilerEvent.CompilerStageEnd || compilerStage == CompilerEvent.Exception)
				{
					string status = "Compiling: " + String.Format("{0:0.00}", (DateTime.Now - builder.CompileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + info;

					builder.AddOutput(status);
				}
				else if (compilerStage == CompilerEvent.Counter)
				{
					builder.AddCounters(info);
				}
			}
		}

		void ICompilerEventListener.SubmitMethodStatus(int totalMethods, int completedMethods)
		{
			if (builder.BuilderEvent != null)
				builder.BuilderEvent.UpdateProgress(totalMethods, completedMethods);
		}

		public BuilderEventListener(Builder builder)
		{
			this.builder = builder;
		}
	}
}