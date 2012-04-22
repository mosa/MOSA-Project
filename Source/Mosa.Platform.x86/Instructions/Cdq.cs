/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cdq instruction.
	/// </summary>
	public sealed class Cdq : X86Instruction
	{
		
		#region Methods


		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public override RegisterBitmap AdditionalInputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0x99);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cdq(context);
		}

		#endregion //  Methods
	}
}
