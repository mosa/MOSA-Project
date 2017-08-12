// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Store Float R4
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class StoreFloatR4 : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="StoreFloatR4"/>.
		/// </summary>
		public StoreFloatR4() :
			base(3, 0)
		{
		}

		#endregion Construction
	}
}
