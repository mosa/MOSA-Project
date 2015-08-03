// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Mov instruction.
	/// </summary>
	public sealed class Mov_store : X86Instruction
	{
		#region Data Members

		private static readonly OpCode RM_C = new OpCode(new byte[] { 0xC7 }, 0); // Move imm32 to r/m32
		private static readonly OpCode RM_C_U8 = new OpCode(new byte[] { 0xC6 }, 0); // Move imm8 to r/m8
		private static readonly OpCode RM_R_U8 = new OpCode(new byte[] { 0x88 });
		private static readonly OpCode R_RM = new OpCode(new byte[] { 0x8B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x89 });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x89 });
		private static readonly OpCode M_C_16 = new OpCode(new byte[] { 0x66, 0xC7 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mov_store"/>.
		/// </summary>
		public Mov_store() :
			base(1, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">@No opcode for operand type. [ + destination + ,  + source + )</exception>
		private OpCode ComputeOpCode(InstructionSize size, Operand destination, Operand source)
		{
			Debug.Assert(destination.IsMemoryAddress);
			Debug.Assert(source.IsRegister || source.IsConstant || source.IsSymbol);

			size = BaseMethodCompilerStage.GetInstructionSize(size, destination);

			if (source.IsSymbol)
				return RM_C;

			if (source.IsConstant)
			{
				if (size == InstructionSize.Size8)
					return RM_C_U8;

				if (size == InstructionSize.Size16)
					return M_C_16;

				return RM_C;
			}

			if (source.IsRegister)
			{
				if (size == InstructionSize.Size8)
					return RM_R_U8;

				if (size == InstructionSize.Size16)
					return M_R_16;

				return M_R;
			}

			throw new ArgumentException(@"No opcode for operand type. [" + destination + ", " + source + ")");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Size, node.Result, node.Operand1);
			emitter.Emit(opCode, node.Result, node.Operand1);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Mov(context);
		}

		#endregion Methods
	}
}
