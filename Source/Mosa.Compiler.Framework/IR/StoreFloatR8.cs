// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Stores a value to a memory pointer.
	/// </summary>
	/// <remarks>
	/// The store instruction stores the value in the given memory pointer with offset.
	/// The first operand is the memory base.
	/// The second operand is the memory base offset.
	/// The third is the value to store.
	/// </remarks>
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
