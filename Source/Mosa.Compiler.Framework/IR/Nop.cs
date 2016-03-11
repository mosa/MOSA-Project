// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	///
	/// </summary>
	public sealed class Nop : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Nop"/>.
		/// </summary>
		public Nop() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
