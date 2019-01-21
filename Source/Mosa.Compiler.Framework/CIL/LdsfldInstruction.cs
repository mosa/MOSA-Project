// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldsfld Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public sealed class LdsfldInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdsfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdsfldInstruction(OpCode opcode)
			: base(opcode, 0, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			var field = (MosaField)decoder.Instruction.Operand;

			Debug.Assert(field.IsStatic, "Static field access on non-static field.");

			node.MosaField = field;
			node.Result = AllocateVirtualRegisterOrStackSlot(decoder.MethodCompiler, field.FieldType);
		}

		#endregion Methods
	}
}
