/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Lea : TwoOperandInstruction
	{
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="emitter"></param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			RegisterOperand rop = (RegisterOperand)context.Result;
			MemoryOperand mop = (MemoryOperand)context.Operand1;
			byte[] code;

			if (mop.Base != null)
			{
				code = new byte[] { 0x8D, 0x84, (4 << 3) };
				code[1] |= (byte)((rop.Register.RegisterCode & 0x07));
				code[2] |= (byte)((mop.Base.RegisterCode & 0x07));
			}
			else
			{
				code = new byte[] { 0xB8 };
			}

			emitter.Write(code, 0, code.Length);
			emitter.EmitImmediate(mop);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Lea(context);
		}

		#endregion // Methods

	}
}
