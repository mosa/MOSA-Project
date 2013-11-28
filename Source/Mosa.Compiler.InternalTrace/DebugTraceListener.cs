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
	public class DebugTraceListener : ITraceListener
	{
		void ITraceListener.SubmitInstructionTraceInformation(RuntimeMethod method, string stage, string line)
		{
			Debug.WriteLine(line);
		}

		void ITraceListener.SubmitDebugStageInformation(RuntimeMethod method, string stage, string line)
		{
			Debug.WriteLine(stage + ": " + line);
		}
	}
}