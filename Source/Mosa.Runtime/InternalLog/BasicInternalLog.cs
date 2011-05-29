/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalLog
{
	public class BasicInternalLog : IInternalLog
	{
		protected IInstructionLogListener instructionLogListener;
		protected IInstructionLogFilter instructionLogFilter = new ConfigurableInstructionLogFilter();
		protected ICompilerStatusListener compilerStatusListener;

		public BasicInternalLog()
		{
			instructionLogListener = new DebugInstructionLogListener();
			compilerStatusListener = new DebugCompilerStatusListener();
		}

		public BasicInternalLog(IInstructionLogListener instructionLogListener, ICompilerStatusListener compilerStatusListener)
		{
			this.instructionLogListener = instructionLogListener;
			this.compilerStatusListener = compilerStatusListener;
		}

		IInstructionLogFilter IInternalLog.InstructionLogFilter
		{
			get { return instructionLogFilter; }
			set { instructionLogFilter = value; }
		}

		IInstructionLogListener IInternalLog.InstructionLogListener
		{
			get { return instructionLogListener; }
			set { instructionLogListener = value; }
		}

		ICompilerStatusListener IInternalLog.CompilerStatusListener
		{
			get { return compilerStatusListener; }
			set { compilerStatusListener = value; }
		}
	}
}
