// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldmfd32 - Load Multiple Full Descending
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Ldmfd32 : ARMv6Instruction
	{
		public override int ID { get { return 713; } }

		internal Ldmfd32()
			: base(1, 3)
		{
		}
	}
}
