// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Trace.BuiltIn
{
	public sealed class DebugCompilerEventListener : ITraceListener
	{
		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string info, int threadID)
		{
			Debug.WriteLine(compilerStage.ToString() + ": " + info);
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}
	}
}