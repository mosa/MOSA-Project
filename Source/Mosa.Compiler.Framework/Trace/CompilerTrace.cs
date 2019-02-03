// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace.BuiltIn;

namespace Mosa.Compiler.Framework.Trace
{
	public class CompilerTrace
	{
		public ITraceListener TraceListener { get; set; }

		public TraceFilter TraceFilter { get; }

		//public int MinTraceLevel { get; set; } = 0;

		public CompilerTrace()
		{
			TraceListener = new DebugCompilerEventListener();
			TraceFilter = new TraceFilter();
		}

		public void NewTraceLog(TraceLog traceLog, bool signalStatusUpdate = false)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnNewTraceLog(traceLog);

			if (signalStatusUpdate)
			{
				TraceListener.OnNewCompilerTraceEvent(CompilerEvent.StatusUpdate, string.Empty, 0);
			}
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
