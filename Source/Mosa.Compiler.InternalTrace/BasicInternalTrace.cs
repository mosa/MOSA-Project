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
		protected ITraceListener instructionLogListener;
		protected ITraceFilter instructionLogFilter = new ConfigurableTraceFilter();
		protected ICompilerEventListener compilerStatusListener;

		public BasicInternalTrace()
		{
			instructionLogListener = new DebugInstructionTraceListener();
			compilerStatusListener = new BasicCompilerEventListener();
		}

		public BasicInternalTrace(ITraceListener instructionLogListener, ICompilerEventListener compilerStatusListener)
		{
			this.instructionLogListener = instructionLogListener;
			this.compilerStatusListener = compilerStatusListener;
		}

		ITraceFilter IInternalTrace.TraceFilter
		{
			get { return instructionLogFilter; }
			set { instructionLogFilter = value; }
		}

		ITraceListener IInternalTrace.TraceListener
		{
			get { return instructionLogListener; }
			set { instructionLogListener = value; }
		}

		ICompilerEventListener IInternalTrace.CompilerEventListener
		{
			get { return compilerStatusListener; }
			set { compilerStatusListener = value; }
		}
	}
}
