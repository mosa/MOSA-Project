// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldstr Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public sealed class LdstrInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdstrInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdstrInstruction(OpCode opCode)
			: base(opCode, 1, 1)
		{
			return;
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

			var token = (uint)decoder.Instruction.Operand;

			string data = decoder.TypeSystem.LookupUserString(decoder.Method.Module, token);

			string symbolName = $"$ldstr${decoder.Method.Module.Name}${token}";

			node.Operand1 = Operand.CreateStringSymbol(symbolName, data, decoder.TypeSystem);

			node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.String);
		}

		#endregion Methods
	}
}
