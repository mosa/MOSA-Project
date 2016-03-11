// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	///
	/// </summary>
	public sealed class Add : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 0);
		private static readonly OpCode M_C = R_C;
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x03 });
		private static readonly OpCode R_R = R_M;
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x01 });

		#endregion Data Members

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
			if (destination.IsRegister && third.IsConstant) return R_C;
			if (destination.IsMemoryAddress && third.IsConstant) return M_C;
			if (destination.IsRegister && third.IsMemoryAddress) return R_M;
			if (destination.IsRegister && third.IsRegister) return R_R;
			if (destination.IsMemoryAddress && third.IsRegister) return M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion Methods
	}
}
