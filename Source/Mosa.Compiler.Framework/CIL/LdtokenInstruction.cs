// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldtoken Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdtokenInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdtokenInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdtokenInstruction(OpCode opcode)
			: base(opcode)
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

			// See Partition III, 4.17 (ldtoken)

			if (decoder.Instruction.Operand is MosaType)
			{
				node.MosaType = (MosaType)decoder.Instruction.Operand;
				node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			}
			else if (decoder.Instruction.Operand is MosaMethod)
			{
				node.InvokeMethod = (MosaMethod)decoder.Instruction.Operand;
				node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeMethodHandle"));
			}
			else if (decoder.Instruction.Operand is MosaField)
			{
				node.MosaField = (MosaField)decoder.Instruction.Operand;
				node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"));
			}
			node.OperandCount = 0;
		}

		#endregion Methods
	}
}
