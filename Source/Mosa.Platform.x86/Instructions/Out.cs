/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 out instruction.
	/// </summary>
	public sealed class Out : X86Instruction
	{
		#region Data Members

		private static readonly OpCode R_8 = new OpCode(new byte[] { 0xEE });
		private static readonly OpCode R_32 = new OpCode(new byte[] { 0xE7 });

		private static readonly OpCode C_8 = new OpCode(new byte[] { 0xE6 });
		private static readonly OpCode C_32 = new OpCode(new byte[] { 0xEF });

		#endregion Data Members

		/// <summary>
		/// Initializes a new instance of <see cref="Out"/>.
		/// </summary>
		public Out() :
			base(0, 2)
		{
		}

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">@No opcode for operand type. [ + destination + ,  + source + )</exception>
		private OpCode ComputeOpCode(InstructionSize size, Operand source, Operand third)
		{
			Debug.Assert(source.IsConstant || source.IsCPURegister);

			//size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			if (source.IsCPURegister)
			{
				if (size == InstructionSize.Size8)
					return R_8;

				return R_32;
			}
			if (source.IsConstant)
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

			if (context.Operand2.IsConstant)
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
			visitor.Out(context);
		}

		#endregion Methods
	}
}