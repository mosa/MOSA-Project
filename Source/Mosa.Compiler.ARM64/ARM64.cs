// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.ARM64.Instructions;

namespace Mosa.Compiler.ARM64;

/// <summary>
/// ARM64 Instructions
/// </summary>
public static class ARM64
{
	public static readonly BaseInstruction Nop = new Nop();
	public static readonly BaseInstruction Adc32 = new Adc32();
	public static readonly BaseInstruction Adc64 = new Adc64();
	public static readonly BaseInstruction Add32 = new Add32();
	public static readonly BaseInstruction Add64 = new Add64();
	public static readonly BaseInstruction And64 = new And64();
	public static readonly BaseInstruction And32 = new And32();
	public static readonly BaseInstruction Asr32 = new Asr32();
	public static readonly BaseInstruction Asr64 = new Asr64();
	public static readonly BaseInstruction B = new B();
}
