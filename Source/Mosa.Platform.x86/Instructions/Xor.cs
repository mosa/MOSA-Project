// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 xor instruction.
	/// </summary>
	public sealed class Xor : TwoOperandInstruction
	{
		#region Data Members

		private static readonly LegacyOpCode R_C = new LegacyOpCode(new byte[] { 0x81 }, 6);
		private static readonly LegacyOpCode R_M = new LegacyOpCode(new byte[] { 0x33 });
		private static readonly LegacyOpCode R_R = new LegacyOpCode(new byte[] { 0x33 });
		private static readonly LegacyOpCode M_R = new LegacyOpCode(new byte[] { 0x31 });
		private static readonly LegacyOpCode M_C = new LegacyOpCode(new byte[] { 0x81 }, 6);

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
			if (destination.IsCPURegister && third.IsConstant) return R_C;
			if (destination.IsCPURegister && third.IsCPURegister) return R_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion Methods
	}
}
