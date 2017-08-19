// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Converts splits a 64-bit integer into two 32-bit integers
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class Split64 : BaseIRInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Split64" />.
		/// </summary>
		public Split64() : base(1, 2)
		{
		}
	}
}
