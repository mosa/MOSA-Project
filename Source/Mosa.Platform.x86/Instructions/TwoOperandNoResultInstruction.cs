/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Abstract base class for x86 instructions with two operands and no result operand.
	/// </summary>
	/// <remarks>
	/// The <see cref="TwoOperandNoResultInstruction"/> is the base class for
	/// x86 instructions using two operands but neither is s result. It 
	/// provides properties to easily access the individual operands.
	/// </remarks>
	public abstract class TwoOperandNoResultInstruction : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="TwoOperandInstruction"/>.
		/// </summary>
		protected TwoOperandNoResultInstruction() :
			base(2, 0)
		{
		}

		#endregion // Construction

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new NotSupportedException();
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
	}
}
