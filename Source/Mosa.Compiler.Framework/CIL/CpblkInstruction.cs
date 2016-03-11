// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class CpblkInstruction : NaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CpblkInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CpblkInstruction(OpCode opcode)
			: base(opcode, 3)
		{
		}

		#endregion Construction
	}
}
