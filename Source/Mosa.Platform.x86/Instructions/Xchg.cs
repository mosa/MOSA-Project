// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 xchg instruction.
	/// </summary>
	public sealed class Xchg : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode R_M = new LegacyOpCode(new byte[] { 0x87 });
		private static readonly LegacyOpCode R_R = new LegacyOpCode(new byte[] { 0x87 });
		private static readonly LegacyOpCode M_R = new LegacyOpCode(new byte[] { 0x87 });
		private static readonly LegacyOpCode R_R_16 = new LegacyOpCode(new byte[] { 0x66, 0x87 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Xchg"/>.
		/// </summary>
		public Xchg() :
			base(1, 1)
		{
		}

		#endregion Construction

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
			if (destination.IsShort && source.IsShort && destination.IsCPURegister && source.IsCPURegister) return R_R_16;
			if (destination.IsCPURegister && source.IsCPURegister) return R_R;

			throw new ArgumentException("No opcode for operand type.");
		}

		#endregion Methods
	}
}
