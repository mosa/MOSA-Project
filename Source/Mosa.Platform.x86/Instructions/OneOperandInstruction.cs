/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Abstract base class for x86 instructions with one operand.
	/// </summary>
	/// <remarks>
	/// The <see cref="OneOperandInstruction"/> is the base class for
	/// x86 instructions using one operand.
	/// </remarks>
	public abstract class OneOperandInstruction : X86Instruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="OneOperandInstruction"/>.
		/// </summary>
		protected OneOperandInstruction() :
			base(1, 0)
		{
		}

		#endregion // Construction

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, null, null);
			emitter.Emit(opCode, context.Result, null);
		}
	}
}
