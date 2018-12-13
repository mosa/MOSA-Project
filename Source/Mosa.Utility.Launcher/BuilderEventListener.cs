// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Framework.Trace;
using System;

namespace Mosa.Utility.Launcher
{
	internal class BuilderEventListener : ITraceListener
	{
		private readonly Builder builder;
		private readonly object _lock = new object();

		public BuilderEventListener(Builder builder)
		{
			this.builder = builder;
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			lock (_lock)
			{
				if (compilerEvent == CompilerEvent.PreCompileStageStart
					|| compilerEvent == CompilerEvent.PreCompileStageEnd
					|| compilerEvent == CompilerEvent.PostCompileStageStart
					|| compilerEvent == CompilerEvent.PostCompileStageEnd
					|| compilerEvent == CompilerEvent.Exception)
				{
					string status = "Compiling: " + String.Format("{0:0.00}", (DateTime.Now - builder.CompileStartTime).TotalSeconds) + " secs: " + compilerEvent.ToText() + ": " + message;

					builder.AddOutput(status);
				}
				else if (compilerEvent == CompilerEvent.Counter)
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

		void ITraceListener.OnMethodcompiled(MosaMethod method)
		{
		}
	}
}
