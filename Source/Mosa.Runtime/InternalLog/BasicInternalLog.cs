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
		protected IInstructionLogListener instructionLogListener = new DebugInstructionLogListener();
		protected IInstructionLogFilter instructionLogFilter = new ConfigurableInstructionLogFilter();
		protected ICompilerStatusListener compilerStatusListener = new DebugCompilerStatusListener();

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
