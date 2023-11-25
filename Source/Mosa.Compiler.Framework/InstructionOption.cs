// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Instruction Options
/// </summary>
[Flags]
public enum InstructionOption
{
	None = 0,
	SetFlags = 1,
	UpDirection = 2,
	Marked = 4,
};
