/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	public class DebugInstructionTraceListener : IInstructionTraceListener 
	{
		void IInstructionTraceListener.NotifyNewInstructionTrace(RuntimeMethod method, string stage, string log)
		{
			//Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", methodCompiler.Method, prevStage.Name));
			Debug.WriteLine(log);
		}

	}
}
