// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// CMovParity32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class CMovParity32 : X64Instruction
	{
		public override int ID { get { return 668; } }

		internal CMovParity32()
			: base(1, 1)
		{
		}

		public override string AlternativeName { get { return "CMovP32"; } }

		public override bool IsParityFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X64.CMovNoParity32;
		}
	}
}
