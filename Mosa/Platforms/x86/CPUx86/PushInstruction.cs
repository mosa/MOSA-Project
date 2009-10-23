/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 push instruction.
	/// </summary>
	public sealed class PushInstruction : OneOperandInstruction
	{
		#region Data Members

		private static readonly OpCode PUSH = new OpCode(new byte[] { 0xFF });
        private static readonly OpCode Const = new OpCode(new byte[] { 0x68 });

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 3; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
            if (ctx.Result is ConstantOperand)
                emitter.Emit(Const.Code, null, ctx.Result, null); 
			else
                emitter.Emit(PUSH.Code, 6, ctx.Result, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Push(context);
		}

		#endregion // Methods
	}
}
