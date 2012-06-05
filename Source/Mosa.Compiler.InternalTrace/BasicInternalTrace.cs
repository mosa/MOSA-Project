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
	public class BasicInternalTrace : IInternalTrace
	{
		protected ITraceListener traceListener;
		protected ITraceFilter traceFilter = new ConfigurableTraceFilter();
		protected ICompilerEventListener compilerEventListener;

		public BasicInternalTrace()
		{
			traceListener = new DebugTraceListener();
			compilerEventListener = new BasicCompilerEventListener();
		}

		public BasicInternalTrace(ITraceListener traceListener, ICompilerEventListener compilerEventListener)
		{
			this.traceListener = traceListener;
			this.compilerEventListener = compilerEventListener;
		}

		ITraceFilter IInternalTrace.TraceFilter
		{
			get { return traceFilter; }
			set { traceFilter = value; }
		}

		ITraceListener IInternalTrace.TraceListener
		{
			get { return traceListener; }
			set { traceListener = value; }
		}

		ICompilerEventListener IInternalTrace.CompilerEventListener
		{
			get { return compilerEventListener; }
			set { compilerEventListener = value; }
		}
	}
}
