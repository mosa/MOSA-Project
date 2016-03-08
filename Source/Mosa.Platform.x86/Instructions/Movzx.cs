// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Movzx instruction.
	/// </summary>
	public sealed class Movzx : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_X8 = new OpCode(new byte[] { 0x0F, 0xB6 });
		private static readonly OpCode R_X16 = new OpCode(new byte[] { 0x0F, 0xB7 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Movzx" />.
		/// </summary>
		public Movzx() :
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
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (!(destination.IsRegister))
				throw new ArgumentException(@"Destination must be RegisterOperand.", @"destination");

			if (source.IsConstant)
				throw new ArgumentException(@"Source must not be ConstantOperand.", @"source");

			if (source.IsByte || source.IsBoolean)
			{
				if (destination.IsRegister && source.IsRegister) return R_X8;
				if (destination.IsRegister && source.IsMemoryAddress) return R_X8;
			}

			if (source.IsShort || source.IsChar)
			{
				if (destination.IsRegister && source.IsRegister) return R_X16;
				if (destination.IsRegister && source.IsMemoryAddress) return R_X16;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination.GetType() + ", " + source.GetType() + ")");
		}

		#endregion Methods
	}
}
