﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Trace.BuiltIn
{
	public sealed class DebugCompilerEventListener : ITraceListener
	{
		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			Debug.WriteLine(compilerEvent.ToString() + ": " + message);
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}

		void ITraceListener.OnMethodCompiled(MosaMethod method)
		{
		}
	}
}
