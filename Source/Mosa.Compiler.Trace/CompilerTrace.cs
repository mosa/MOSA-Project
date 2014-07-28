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
	public class CompilerTrace
	{
		public ITraceListener TraceListener { get; set; }

		public ITraceFilter TraceFilter { get; set; }

		public ICompilerEventListener CompilerEventListener { get; set; }

		public CompilerTrace()
		{
			CompilerEventListener = new DebugCompilerEventListener();
			TraceListener = new DebugTraceListener();
			TraceFilter = new ConfigurableTraceFilter();
		}

	}
}