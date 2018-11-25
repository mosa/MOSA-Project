// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System;

namespace Mosa.Platform.x64
{
	/// <summary>
	/// X86Instruction
	/// </summary>
	public abstract class X64Instruction : BasePlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="X64Instruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected X64Instruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string FamilyName { get { return "X64"; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			EmitLegacy(node, emitter as X64CodeEmitter);
		}

		#endregion Methods

		#region Legacy Opcode Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		internal virtual void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)
		{
			var opCode = ComputeOpCode(node.Result, node.Operand1, node.Operand2);
			emitter.Emit(opCode, node.Result, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">opcode not implemented for this instruction</exception>
		internal virtual LegacyOpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new Exception("opcode not implemented for this instruction");
		}

		#endregion Legacy Opcode Methods
	}
}
