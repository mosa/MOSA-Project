// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using System.Collections.Generic;

namespace Mosa.Tool.GDBDebugger.GDB
{
	public abstract class BasePlatform
	{
		public List<Register> Registers { get; protected set; }

		internal abstract void Parse(GDBCommand command);

		public abstract Register InstructionPointer { get; }
		public abstract Register StackPointer { get; }
		public abstract Register StackFrame { get; }
		public abstract Register StatusFlag { get; }
	}
}
