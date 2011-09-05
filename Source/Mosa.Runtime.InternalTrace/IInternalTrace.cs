/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */



namespace Mosa.Runtime.InternalTrace
{
	public interface IInternalTrace
	{
		IInstructionTraceListener InstructionTraceListener { get; set; }
		IInstructionTraceFilter InstructionTraceFilter { get; set; }
		ICompilerEventListener CompilerEventListener { get; set; }
	}
}
