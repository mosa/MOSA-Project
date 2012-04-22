/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */


using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Leave : X86Instruction
	{

		#region Methods

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.ESP, GeneralPurposeRegister.EBP); } }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public override RegisterBitmap AdditionalInputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EBP); } }

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xC9);
		}

		#endregion // Methods

	}
}
