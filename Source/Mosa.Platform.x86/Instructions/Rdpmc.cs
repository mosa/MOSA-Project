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
	/// Representations the x86 rdpmc instruction.
	/// </summary>
	public sealed class Rdpmc : X86Instruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rdtsc"/>.
		/// </summary>
		public Rdpmc() :
			base(0, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public override RegisterBitmap AdditionalOutputRegisters { get { return new RegisterBitmap(GeneralPurposeRegister.EDX, GeneralPurposeRegister.EAX); } }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rdpmc(context);
		}

		#endregion // Methods
	}
}
