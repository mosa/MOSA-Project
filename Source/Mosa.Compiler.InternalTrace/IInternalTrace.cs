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
	public interface IInternalTrace
	{
		ITraceListener TraceListener { get; set; }
		ITraceFilter TraceFilter { get; set; }
		ICompilerEventListener CompilerEventListener { get; set; }
	}
}
