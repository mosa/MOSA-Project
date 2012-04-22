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
	/// Representations of the x86 stos instruction.
	/// </summary>
	public sealed class Stos : X86Instruction
	{

		#region Methods

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public override RegisterBitmap AdditionalInputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.ECX, GeneralPurposeRegister.EDI, GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EDI); } }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xAB);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Stosd(context);
		}

		#endregion // Methods
	}
}
