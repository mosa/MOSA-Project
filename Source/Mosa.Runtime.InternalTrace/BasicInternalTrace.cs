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
	public class BasicInternalTrace : IInternalTrace
	{
		protected IInstructionTraceListener instructionLogListener;
		protected IInstructionTraceFilter instructionLogFilter = new ConfigurableInstructionTraceFilter();
		protected ICompilerEventListener compilerStatusListener;

		public BasicInternalTrace()
		{
			instructionLogListener = new DebugInstructionTraceListener();
			compilerStatusListener = new BasicCompilerEventListener();
		}

		public BasicInternalTrace(IInstructionTraceListener instructionLogListener, ICompilerEventListener compilerStatusListener)
		{
			this.instructionLogListener = instructionLogListener;
			this.compilerStatusListener = compilerStatusListener;
		}

		IInstructionTraceFilter IInternalTrace.InstructionTraceFilter
		{
			get { return instructionLogFilter; }
			set { instructionLogFilter = value; }
		}

		IInstructionTraceListener IInternalTrace.InstructionTraceListener
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
