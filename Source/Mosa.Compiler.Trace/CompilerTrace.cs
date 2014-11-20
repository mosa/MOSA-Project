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
		private ITraceListener traceListener;
		private ICompilerEventListener compilerEventListener;
		private object traceListenerLock = new object();
		private object compilerEventListenerLock = new object();

		public ITraceListener TraceListener
		{
			get
			{
				lock(traceListenerLock)
				{
					return traceListener;
				}
			}
			set { traceListener = value; }
		}

		public ITraceFilter TraceFilter { get; set; }

		public ICompilerEventListener CompilerEventListener
		{
			get
			{
				lock (compilerEventListenerLock)
				{
					return compilerEventListener;
				}
			}
			set { compilerEventListener = value; }
		}

		public CompilerTrace()
		{
			CompilerEventListener = new DebugCompilerEventListener();
			TraceListener = new DebugTraceListener();
			TraceFilter = new ConfigurableTraceFilter();
		}

	}
}