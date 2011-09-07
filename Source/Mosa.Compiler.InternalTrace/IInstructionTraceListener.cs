/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalTrace
{

	public interface IInstructionTraceListener
	{
		void NotifyNewInstructionTrace(RuntimeMethod method, string stage, string log);
	}
}
