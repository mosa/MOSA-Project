// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 adc instruction.
	/// </summary>
	public sealed class Adc : TwoOperandInstruction
	{
		#region Data members

		private static readonly LegacyOpCode RM_C = new LegacyOpCode(new byte[] { 0x81 }, 2);
		private static readonly LegacyOpCode R_RM = new LegacyOpCode(new byte[] { 0x13 });
		private static readonly LegacyOpCode M_R = new LegacyOpCode(new byte[] { 0x11 });

		#endregion Data members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		internal override LegacyOpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (destination.IsCPURegister && third.IsConstant) return RM_C;
			if (destination.IsCPURegister && third.IsCPURegister) return R_RM;

			throw new ArgumentException("No opcode for operand type.");
		}

		#endregion Methods
	}
}
