// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Trace.BuiltIn;

namespace Mosa.Compiler.Trace
{
	public class CompilerTrace
	{
		public ITraceListener TraceListener { get; set; }

		public TraceFilter TraceFilter { get; private set; }

		public CompilerTrace()
		{
			TraceListener = new DebugCompilerEventListener();
			TraceFilter = new TraceFilter();
		}

		public void NewTraceLog(TraceLog traceLog)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnNewTraceLog(traceLog);
		}

		public void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnNewCompilerTraceEvent(compilerEvent, message, threadID);
		}

		public void UpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnUpdatedCompilerProgress(totalMethods, completedMethods);
		}
	}
}
