// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Trace;
using System;

namespace Mosa.Utility.Launcher
{
	internal class BuilderEventListener : ITraceListener
	{
		private readonly Builder builder;
		private readonly object mylock = new object();

		public BuilderEventListener(Builder builder)
		{
			this.builder = builder;
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string message, int threadID)
		{
			lock (mylock)
			{
				if (compilerStage == CompilerEvent.PreCompileStageStart
					|| compilerStage == CompilerEvent.PreCompileStageEnd
					|| compilerStage == CompilerEvent.PostCompileStageStart
					|| compilerStage == CompilerEvent.PostCompileStageEnd
					|| compilerStage == CompilerEvent.Exception)
				{
					string status = "Compiling: " + String.Format("{0:0.00}", (DateTime.Now - builder.CompileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + message;

					builder.AddOutput(status);
				}
				else if (compilerStage == CompilerEvent.Counter)
				{
					builder.AddCounters(message);
				}
			}
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			builder.BuilderEvent?.UpdateProgress(totalMethods, completedMethods);
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}
	}
}
