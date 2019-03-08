﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Trace
{
	public class CompilerTrace
	{
		private ITraceListener TraceListener;

		public int TraceLevel { get; set; } = 0;

		public CompilerTrace()
		{
			TraceListener = null;
		}

		public bool IsTraceable(int traceLevel)
		{
			return TraceLevel != 0 && TraceLevel >= traceLevel;
		}

		public void SetTraceListener(ITraceListener traceListener)
		{
			TraceListener = traceListener;
		}

		public void PostMethodCompiled(MosaMethod method)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnMethodCompiled(method);
		}

		public void PostTraceLog(TraceLog traceLog, bool signalStatusUpdate = false)
		{
			if (!traceLog.Active)
				return;

			if (TraceListener == null)
				return;

			TraceListener.OnTraceLog(traceLog);

			if (signalStatusUpdate)
			{
				TraceListener.OnCompilerEvent(CompilerEvent.StatusUpdate, string.Empty, 0);
			}
		}

		public void PostCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnCompilerEvent(compilerEvent, message, threadID);
		}

		public void UpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
			if (TraceListener == null)
				return;

			TraceListener.OnProgress(totalMethods, completedMethods);
		}
	}
}
