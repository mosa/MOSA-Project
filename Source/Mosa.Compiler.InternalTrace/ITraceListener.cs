/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	public interface ITraceListener
	{
		void SubmitInstructionTraceInformation(MosaMethod method, string stage, string line);

		void SubmitDebugStageInformation(MosaMethod method, string stage, string line);
	}
}