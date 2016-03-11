// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class AddInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddInstruction"/> class.
		/// </summary>
		public AddInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction
	}
}
