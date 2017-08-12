// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of the not instruction.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseTwoOperandInstruction" />
	public sealed class LogicalNot : BaseTwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="LogicalNot"/>.
		/// </summary>
		public LogicalNot()
		{
		}

		#endregion Construction
	}
}
