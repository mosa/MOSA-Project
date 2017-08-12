// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of the method prologue.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	/// <remarks>
	/// This instruction is usually derived by the architecture and expanded appropriately
	/// for the calling convention of the method.
	/// </remarks>
	public sealed class Prologue : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Prologue"/>.
		/// </summary>
		public Prologue() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
