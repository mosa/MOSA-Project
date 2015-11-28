// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of the call to the finally block.
	/// </summary>
	public sealed class Leave : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Leave"/>.
		/// </summary>
		public Leave() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
