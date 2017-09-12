// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Memory Set
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class MemorySet : BaseIRInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemorySet" />.
		/// </summary>
		public MemorySet() :
			base(3, 1)
		{
		}
	}
}
