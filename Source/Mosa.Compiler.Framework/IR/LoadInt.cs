// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Loads a value from a memory pointer.
	/// </summary>
	/// <remarks>
	/// The load instruction is used to load a value from a memory pointer and an offset. The types must be compatible.
	/// </remarks>
	public sealed class LoadInt : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="LoadInt"/>.
		/// </summary>
		public LoadInt()
		{
		}

		#endregion Construction
	}
}
