// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Callvirt Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.InvokeInstruction" />
	public sealed class CallvirtInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallvirtInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CallvirtInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
		}

		#endregion Properties

		#region Methods

		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			base.Decode(node, decoder);
		}

		#endregion Methods
	}
}
