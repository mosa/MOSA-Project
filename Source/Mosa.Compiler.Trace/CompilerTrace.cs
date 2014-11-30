/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

		public void NewCompilerTraceEvent(CompilerEvent compilerStage, string message, int threadID)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnNewCompilerTraceEvent(compilerStage, message, threadID);
		}

		public void UpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnUpdatedCompilerProgress(totalMethods, completedMethods);
		}
	}
}