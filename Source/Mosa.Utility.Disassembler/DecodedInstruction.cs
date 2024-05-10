// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.Disassembler;

public class DecodedInstruction
{
	public ulong Address { get; init; }

	public uint Length { get; init; }

	public string Instruction { get; init; }

	public string Full { get; init; }
}
