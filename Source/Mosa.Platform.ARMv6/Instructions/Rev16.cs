/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Marcelo Caetano (marcelocaetano) <marcelo.caetano@ymail.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Rev16 instruction: Byte-Reverse Packed Halfword
	/// </summary>
	public class Rev16 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rev16"/>.
		/// </summary>
		public Rev16() :
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
			visitor.Rev16(context);
		}

		#endregion Methods
	}
}