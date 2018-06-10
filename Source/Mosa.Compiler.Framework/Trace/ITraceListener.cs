// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Trace
{
	public interface ITraceListener
	{
		void OnNewCompilerTraceEvent(CompilerEvent compilerEvent, string message, int threadID);

		void OnUpdatedCompilerProgress(int totalMethods, int completedMethods);

		void OnNewTraceLog(TraceLog traceLog);

		void OnMethodcompiled(MosaMethod method);
	}
}
