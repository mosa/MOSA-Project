// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of a routine to leave a protected block, finally or catch handler to normal flow control, calling finally handlers as appropriate.
	/// Not to be confused with CIL Leave instruction.
	/// </summary>
	public sealed class GotoLeaveTarget : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="GotoLeaveTarget"/>.
		/// </summary>
		public GotoLeaveTarget() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
