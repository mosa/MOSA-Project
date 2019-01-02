// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
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
			if (compilerEvent == CompilerEvent.PreCompileStageStart
				|| compilerEvent == CompilerEvent.PreCompileStageEnd
				|| compilerEvent == CompilerEvent.PostCompileStageStart
				|| compilerEvent == CompilerEvent.PostCompileStageEnd
				|| compilerEvent == CompilerEvent.Exception)
			{
				string status = $"Compiling: {$"{(DateTime.Now - builder.CompileStartTime).TotalSeconds:0.00}"} secs: {compilerEvent.ToText()}: {message}";

				lock (_lock)
				{
					builder.AddOutput(status);
				}
			}
			else if (compilerEvent == CompilerEvent.Counter)
			{
				lock (_lock)
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
