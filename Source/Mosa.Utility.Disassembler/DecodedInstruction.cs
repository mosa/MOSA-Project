// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.Disassembler
{
	public partial class DecodedInstruction
	{
		public ulong Address { get; set; }

		public int Length { get; set; }

		public string Instruction { get; set; }

		public string Full { get; set; }
	}
}
