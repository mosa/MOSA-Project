// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 pause instruction.
	/// </summary>
	public sealed class Rdtsc : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rdtsc"/>.
		/// </summary>
		public Rdtsc() :
			base(1, 0)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Rdtsc(context);
		}

		#endregion Methods
	}
}