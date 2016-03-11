// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cld instruction.
	/// </summary>
	public sealed class Cld : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Cld"/>.
		/// </summary>
		public Cld() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
