// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for IR instructions with three operands.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	/// <remarks>
	/// The <see cref="BaseThreeOperandInstruction" /> is the base class for
	/// IR instructions using three operands. It provides properties to
	/// easily access the individual operands.
	/// </remarks>
	public abstract class BaseThreeOperandInstruction : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseThreeOperandInstruction"/>.
		/// </summary>
		protected BaseThreeOperandInstruction() :
			base(2, 1)
		{
		}

		#endregion Construction
	}
}
