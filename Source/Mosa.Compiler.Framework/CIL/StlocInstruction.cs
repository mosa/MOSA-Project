// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Stloc Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.StoreInstruction" />
	public sealed class StlocInstruction : StoreInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StlocInstruction" /> class.
		/// </summary>
		/// <param name="opcode"></param>
		public StlocInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Stloc has a result, but doesn't push it on the stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public override bool PushResult
		{
			get { return false; }
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <exception cref="NotImplementCompilerException"></exception>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			int index;

			// Destination depends on the opcode
			switch (opcode)
			{
				case OpCode.Stloc:
				case OpCode.Stloc_s: index = (int)decoder.Instruction.Operand; break;
				case OpCode.Stloc_0: index = 0; break;
				case OpCode.Stloc_1: index = 1; break;
				case OpCode.Stloc_2: index = 2; break;
				case OpCode.Stloc_3: index = 3; break;
				default: throw new NotImplementCompilerException();
			}

			node.Result = decoder.MethodCompiler.LocalVariables[index];
		}

		#endregion Methods
	}
}
