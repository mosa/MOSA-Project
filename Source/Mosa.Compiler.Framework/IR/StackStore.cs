// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Stores a value to the stack.
	/// </summary>
	/// <remarks>
	/// The store instruction stores the value to the stack.
	/// </remarks>
	public sealed class StackStore : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Store"/>.
		/// </summary>
		public StackStore() :
			base(3, 0)
		{
		}

		#endregion Construction
	}
}
