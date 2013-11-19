/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cmp instruction.
	/// </summary>
	public sealed class Cmp : X86Instruction
	{
		#region Data Member

		private static readonly OpCode M_R = new OpCode(new byte[] { 0x39 });
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x3B });
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x3B });
		private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 7);
		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 7);
		private static readonly OpCode R_C_8 = new OpCode(new byte[] { 0x80 }, 7);
		private static readonly OpCode R_C_16 = new OpCode(new byte[] { 0x66, 0x81 }, 7);
		private static readonly OpCode M_R_8 = new OpCode(new byte[] { 0x38 });
		private static readonly OpCode R_M_8 = new OpCode(new byte[] { 0x3A });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x39 });
		private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x3B });

		#endregion Data Member

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Cmp"/>.
		/// </summary>
		public Cmp() :
			base(0, 2)
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
			if (source.IsMemoryAddress && third.IsRegister)
			{
				if (source.IsByte || third.IsByte)
					return M_R_8;
				if (source.IsChar || third.IsChar)
					return M_R_16;
				if (source.IsShort || third.IsShort)
					return M_R_16;
				return M_R;
			}

			if (source.IsRegister && third.IsMemoryAddress)
			{
				if (third.IsByte || source.IsByte)
					return R_M_8;
				if (third.IsChar || source.IsChar)
					return R_M_16;
				if (third.IsShort || source.IsShort)
					return R_M_16;
				return R_M;
			}

			if (source.IsRegister && third.IsRegister) return R_R;

			if (source.IsMemoryAddress && third.IsConstant) return M_C;

			if (source.IsRegister && third.IsConstant)
			{
				if (third.IsByte || source.IsByte)
					return R_C_8;
				if (third.IsChar || source.IsChar)
					return R_C_16;
				if (third.IsShort || source.IsShort)
					return R_C_16;
				return R_C;
			}

			throw new ArgumentException(String.Format(@"x86.Cmp: No opcode for operand types {0} and {1}.", source, third));
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			Debug.Assert(context.Result == null);

			OpCode opCode = ComputeOpCode(null, context.Operand1, context.Operand2);
			emitter.Emit(opCode, context.Operand1, context.Operand2);
		}

		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cmp(context);
		}

		#endregion Methods
	}
}