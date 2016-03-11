// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 rdmsr instruction.
	/// </summary>
	public sealed class Rdmsr : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rdtsc"/>.
		/// </summary>
		public Rdmsr() :
			base(1, 0)
		{
		}

		#endregion Construction
	}
}
