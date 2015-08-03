// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public class StoreInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StlocInstruction"/> class.
		/// </summary>
		public StoreInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction
	}
}