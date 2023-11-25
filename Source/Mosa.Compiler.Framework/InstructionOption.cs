// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Instruction Options
/// </summary>
[Flags]
public enum InstructionOption
{
	None = 0,

	Set = 0b0001,

	UpDirection = 0b10,
	DownDirection = 0b00,

	Marked = 0b100,
};
