/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 pushfd instruction.
	/// </summary>
	public sealed class PushfdInstruction : X86Instruction
	{
		#region Data members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x9C });

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="third"></param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Compiler.Framework.Operands.Operand destination, Compiler.Framework.Operands.Operand source, Compiler.Framework.Operands.Operand third)
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

		#endregion // Methods
	}
}