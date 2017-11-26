// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 inc instruction.
	/// </summary>
	public sealed class Inc : X86Instruction
	{
		#region Data Members

		private static readonly LegacyOpCode INC8 = new LegacyOpCode(new byte[] { 0xFE }, 0);
		private static readonly LegacyOpCode INC16 = new LegacyOpCode(new byte[] { 0x66, 0xFF }, 0);
		private static readonly LegacyOpCode INC32 = new LegacyOpCode(new byte[] { 0xFF }, 0);

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Inc"/>.
		/// </summary>
		public Inc() :
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
			if (destination.IsByte) return INC8;
			if (destination.IsShort || destination.IsChar) return INC16;
			if (destination.IsInt) return INC32;

			throw new ArgumentException("No opcode for operand type.");
		}

		#endregion Methods
	}
}
