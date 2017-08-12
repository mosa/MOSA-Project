// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Store Compound
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class StoreCompound : BaseIRInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StoreCompound"/> class.
		/// </summary>
		public StoreCompound()
			: base(3, 0)
		{
		}
	}
}
