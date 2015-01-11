/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;
using Mosa.Compiler.Common;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	public sealed class In : TwoOperandInstruction
	{
		#region Data Members

		private static readonly OpCode C_8 = new OpCode(new byte[] { 0xE4 });
		private static readonly OpCode R_8 = new OpCode(new byte[] { 0xEC });
		private static readonly OpCode C_32 = new OpCode(new byte[] { 0xE5 });
		private static readonly OpCode R_32 = new OpCode(new byte[] { 0xED });
		private static readonly OpCode opcode = new OpCode(new byte[] { 0xEC });

		#endregion Data Members

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException"></exception>
		private OpCode ComputeOpCode(InstructionSize size, Operand destination, Operand source)
		{
			Debug.Assert(destination.IsConstant || destination.IsCPURegister);

			//size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			if (destination.IsCPURegister)
			{
				if (size == InstructionSize.Size8)
					return R_8;

				return R_32;
			}

			if (destination.IsConstant)
			{
				if (size == InstructionSize.Size8)
					return C_8;

				return C_32;
			}

			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			var opCode = ComputeOpCode(context.Size, context.Operand1, context.Operand2);

			if (context.Operand1.IsConstant)
			{
				emitter.Emit(opCode, context.Operand1, null);
			}
			else
			{
				emitter.Emit(opCode, null, null);
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.In(context);
		}

		#endregion Methods
	}
}
