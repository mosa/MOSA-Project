// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// MovCRLoad32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class MovCRLoad32 : X64Instruction
	{
		public override int ID { get { return 487; } }

		internal MovCRLoad32()
			: base(1, 1)
		{
		}

		public override bool IsMemoryRead { get { return true; } }
	}
}