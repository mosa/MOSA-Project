// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// New Array
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class NewArray : BaseIRInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NewArray" />.
		/// </summary>
		public NewArray() :
			base(2, 1)
		{
		}
	}
}
