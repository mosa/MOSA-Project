// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Binary Comparison Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BinaryInstruction" />
	public sealed class BinaryComparisonInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryComparisonInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BinaryComparisonInstruction(OpCode opcode)
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

			// Set the result
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.MethodCompiler.Architecture.Is32BitPlatform ? decoder.TypeSystem.BuiltIn.I4 : decoder.TypeSystem.BuiltIn.I8);
		}

		/// <summary>
		/// Gets the modifier.
		/// </summary>
		/// <exception cref="InvalidOperationException">Invalid opcode.</exception>
		public override string Modifier
		{
			get
			{
				switch (OpCode)
				{
					case OpCode.Ceq: return "==";
					case OpCode.Cgt: return ">";
					case OpCode.Cgt_un: return "> unordered";
					case OpCode.Clt: return "<";
					case OpCode.Clt_un: return "< unordered";
					default: throw new InvalidOperationException("Invalid opcode.");
				}
			}
		}

		#endregion Methods
	}
}
