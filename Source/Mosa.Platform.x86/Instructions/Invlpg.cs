/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Invlpg instruction.
	/// </summary>
	public sealed class Invlpg : X86Instruction
	{
		#region Data Members

		private static readonly OpCode INVLPG = new OpCode(new byte[] { 0x0F, 0x01 }, 7);

		#endregion // Data Members
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Invlpg"/>.
		/// </summary>
		public Invlpg() :
			base(1, 0)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.Emit(INVLPG, context.Operand1, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Invlpg(context);
		}
		
		#endregion // Methods
	}
}
