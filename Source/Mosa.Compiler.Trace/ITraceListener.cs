/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Trace
{
	public interface ITraceListener
	{
		void OnNewCompilerTraceEvent(CompilerEvent compilerStage, string message, int threadID);

		void OnUpdatedCompilerProgress(int totalMethods, int completedMethods);

		void OnNewTraceLog(TraceLog traceLog);
	}
}