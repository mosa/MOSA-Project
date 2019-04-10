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

		void ITraceListener.OnCompilerEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent == CompilerEvent.CompilerStart
				|| compilerEvent == CompilerEvent.CompilerEnd
				|| compilerEvent == CompilerEvent.CompilingMethods
				|| compilerEvent == CompilerEvent.CompilingMethodsCompleted
				|| compilerEvent == CompilerEvent.InlineMethodsScheduled
				|| compilerEvent == CompilerEvent.LinkingStart
				|| compilerEvent == CompilerEvent.LinkingEnd
				|| compilerEvent == CompilerEvent.Warning
				|| compilerEvent == CompilerEvent.Error
				|| compilerEvent == CompilerEvent.Exception)
			{
				string status = $"Compiling: {$"{(DateTime.Now - builder.CompileStartTime).TotalSeconds:0.00}"} secs: {compilerEvent.ToText()}";

				if (!string.IsNullOrEmpty(message))
					status += $"- { message}";

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

		void ITraceListener.OnProgress(int totalMethods, int completedMethods)
		{
			builder.BuilderEvent?.UpdateProgress(totalMethods, completedMethods);
		}

		void ITraceListener.OnTraceLog(TraceLog traceLog)
		{
		}

		void ITraceListener.OnMethodCompiled(MosaMethod method)
		{
		}
	}
}
