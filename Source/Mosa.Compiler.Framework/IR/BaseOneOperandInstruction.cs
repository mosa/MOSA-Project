// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for IR instructions with one operand.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public abstract class BaseOneOperandInstruction : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseOneOperandInstruction"/> class.
		/// </summary>
		protected BaseOneOperandInstruction() :
			base(1, 0)
		{
		}

		#endregion Construction
	}
}
