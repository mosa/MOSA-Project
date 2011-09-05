/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platform.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 rep instruction.
	/// </summary>
	public sealed class RepInstruction : BaseInstruction
	{
		#region Methods

		/// <summary>
		/// Emits the specified CTX.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xF3);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rep(context);
		}

		#endregion // Methods
	}
}
