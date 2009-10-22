/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representation the x86 jump instruction.
	/// </summary>
	public sealed class JmpInstruction : BaseInstruction
	{

		#region Data Members

		private static readonly OpCode JMP = new OpCode(new byte[] { 0xE9 });

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			emitter.EmitBranch(JMP.Code, ctx.Branch.Targets[0]);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Add(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"{0} L_{1:X4}", base.ToString(context), context.Branch.Targets[0]);
		}

		#endregion // Methods

	}
}
