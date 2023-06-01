// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Utility.RSP;

namespace Mosa.Tool.Debugger.GDB;

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
		return size switch
		{
			1 => $"0x{(uint)value:X2}",
			2 => $"0x{(uint)value:X4}",
			4 => $"0x{(uint)value:X8}",
			8 => $"0x{value:X16}",
			16 => $"0x{value:X32}",
			_ => "N/A",
		};
	}

	public abstract uint FirstPrologueInstructionSize { get; }
}
