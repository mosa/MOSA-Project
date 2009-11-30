/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 pop instruction.
	/// </summary>
	public sealed class PopInstruction : BaseInstruction
	{
		#region Data Members

		private static readonly OpCode POP = new OpCode(new byte[] { 0x8F });

		#endregion

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
            if (ctx.Result is RegisterOperand)
				emitter.WriteByte((byte)(0x58 + (ctx.Result as RegisterOperand).Register.RegisterCode));
			else
                emitter.Emit(POP.Code, 0, ctx.Result, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Pop(context);
		}

		#endregion // Methods
	}
}
