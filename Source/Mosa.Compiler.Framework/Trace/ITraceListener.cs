// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Trace
{
	public interface ITraceListener
	{
		void OnCompilerEvent(CompilerEvent compilerEvent, string message, int threadID);

		void OnProgress(int totalMethods, int completedMethods);

		void OnTraceLog(TraceLog traceLog);

		void OnMethodCompiled(MosaMethod method);
	}
}
