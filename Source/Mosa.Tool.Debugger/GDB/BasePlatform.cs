// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.RSP;
using System.Collections.Generic;

namespace Mosa.Tool.Debugger.GDB
{
	public abstract class BasePlatform
	{
		public List<Register> Registers { get; protected set; }

		internal abstract void Parse(GDBCommand command);

		public abstract uint NativeIntegerSize { get; }

		public abstract Register InstructionPointer { get; }
		public abstract Register StackPointer { get; }
		public abstract Register StackFrame { get; }
		public abstract Register StatusFlag { get; }

		public static string ToHex(long value, uint size)
		{
			return ToHex((ulong)value, size);
		}

		public static string ToHex(ulong value, uint size)
		{
			switch (size)
			{
				case 1: return "0x" + ((uint)value).ToString("X2");
				case 2: return "0x" + ((uint)value).ToString("X4");
				case 4: return "0x" + ((uint)value).ToString("X8");
				case 8: return "0x" + value.ToString("X16");
				default: return "N/A";
			}
		}

		public abstract uint FirstPrologueInstructionSize { get; }
	}
}
