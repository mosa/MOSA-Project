// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Memory Copy
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class MemoryCopy : BaseIRInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemoryCopy" />.
		/// </summary>
		public MemoryCopy() :
			base(3, 1)
		{
		}
	}
}
