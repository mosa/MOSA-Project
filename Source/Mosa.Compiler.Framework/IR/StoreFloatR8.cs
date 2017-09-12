// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Store Float R8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class StoreFloatR8 : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="StoreFloatR8"/>.
		/// </summary>
		public StoreFloatR8() :
			base(3, 0)
		{
		}

		#endregion Construction
	}
}
