/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Platform.ARMv6;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldrh instruction: Load 16-bit unsigned halfword
	/// Load and store instructions
	/// </summary>
	public class Ldrh : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Ldrh"/>.
		/// </summary>
		public Ldrh() :
			base(1, 3)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			// TODO
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IARMv6Visitor visitor, Context context)
		{
			visitor.Ldrh(context);
		}

		#endregion Methods
	}
}