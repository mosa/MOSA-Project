// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Uxtb32 - Unsigned Extend Byte
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Uxtb32 : ARMv6Instruction
	{
		public override int ID { get { return 751; } }

		internal Uxtb32()
			: base(1, 3)
		{
		}
	}
}
