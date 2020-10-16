// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;

namespace Mosa.Tool.Debugger.GDB
{
	public class X86Platform : BasePlatform
	{
		internal override void Parse(GDBCommand command)
		{
			Registers = X86.Parse(command);
		}

		public override uint NativeIntegerSize { get { return 4; } }
		public override Register InstructionPointer { get { return Registers[X86.InstructionPointerIndex]; } }
		public override Register StackPointer { get { return Registers[X86.StackPointerIndex]; } }
		public override Register StackFrame { get { return Registers[X86.StackFrameIndex]; } }
		public override Register StatusFlag { get { return Registers[X86.StatusFlagIndex]; } }

		public override uint FirstPrologueInstructionSize { get { return 2; } }
	}
}
