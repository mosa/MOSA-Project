/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.InternalTrace
{
	public interface ICompilerEventListener
	{
		void SubmitTraceEvent(CompilerEvent compilerStage, string info);

		void SubmitMethodStatus(int totalMethods, int queuedMethods);
	}
}