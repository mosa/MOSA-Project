// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86
{
	/// <summary>
	///
	/// </summary>
	public abstract class X86Instruction : BasePlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected X86Instruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets a value indicating whether [three two address conversion].
		/// </summary>
		/// <value>
		/// <c>true</c> if [three two address conversion]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ThreeTwoAddressConversion { get { return true; } }

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string InstructionFamilyName { get { return "X86"; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			Emit(node, emitter as MachineCodeEmitter);
		}

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected virtual OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new System.Exception("opcode not implemented for this instruction");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected virtual void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(node.Result, node.Operand1, node.Operand2);
			emitter.Emit(opCode, node.Result, node.Operand1, node.Operand2);
		}

		#endregion Methods
	}
}
