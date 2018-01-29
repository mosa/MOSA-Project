// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cvtsi2ss instruction.
	/// </summary>
	public sealed class Cvtsi2ss : X86Instruction
	{
		#region Data members

		private static readonly LegacyOpCode opcode = new LegacyOpCode(new byte[] { 0xF3, 0x0F, 0x2A });

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Cvtsi2ss"/>.
		/// </summary>
		public Cvtsi2ss() :
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
			if (destination.IsCPURegister && source.IsCPURegister) return opcode;
			throw new ArgumentException("No opcode for operand type.");
		}

		#endregion Methods
	}
}
