// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the sub instruction.
	/// </summary>
	public sealed class Sub : TwoOperandInstruction
	{
		#region Data Members

		private static readonly LegacyOpCode O_C = new LegacyOpCode(new byte[] { 0x81 }, 5);
		private static readonly LegacyOpCode R_O = new LegacyOpCode(new byte[] { 0x2B });
		private static readonly LegacyOpCode R_O_16 = new LegacyOpCode(new byte[] { 0x66, 0x2B });
		private static readonly LegacyOpCode M_R = new LegacyOpCode(new byte[] { 0x29 });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		internal override LegacyOpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (third.IsConstant)
				return O_C;

			if (destination.IsCPURegister)
			{
				if (third.IsChar)
					return R_O_16;
				else
					return R_O;
			}

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion Methods
	}
}
