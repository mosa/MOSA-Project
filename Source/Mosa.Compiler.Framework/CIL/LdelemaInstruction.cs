﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.Units;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldelema Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BinaryInstruction" />
	public sealed class LdelemaInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdelemaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdelemaInstruction(OpCode opcode)
			: base(opcode, 1)
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

			var type = (MosaType)decoder.Instruction.Operand;

			node.Result = decoder.MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
		}

		#endregion Methods
	}
}
