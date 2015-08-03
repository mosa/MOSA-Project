// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Trace
{
	public interface ITraceListener
	{
		void OnNewCompilerTraceEvent(CompilerEvent compilerStage, string message, int threadID);

		void OnUpdatedCompilerProgress(int totalMethods, int completedMethods);

		void OnNewTraceLog(TraceLog traceLog);
	}
}