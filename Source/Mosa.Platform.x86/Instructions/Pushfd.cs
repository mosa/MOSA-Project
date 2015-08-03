// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 pushfd instruction.
	/// </summary>
	public sealed class Pushfd : X86Instruction
	{
		#region Data members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x9C });

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Pushfd"/>.
		/// </summary>
		public Pushfd() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		///
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			return opcode;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Pushfd(context);
		}

		#endregion Methods
	}
}