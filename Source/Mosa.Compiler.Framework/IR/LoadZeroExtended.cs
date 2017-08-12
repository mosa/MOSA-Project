// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Load Zero Extended
	/// </summary>
	/// <remarks>
	/// The load instruction is used to load a value from a memory pointer and an offset. The types must be compatible.
	/// </remarks>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseThreeOperandInstruction" />
	public sealed class LoadZeroExtended : BaseThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="LoadZeroExtended"/>.
		/// </summary>
		public LoadZeroExtended()
		{
		}

		#endregion Construction
	}
}
