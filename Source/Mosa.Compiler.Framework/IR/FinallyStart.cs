// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// FinallyStart
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class FinallyStart : BaseIRInstruction
	{
		public FinallyStart()
			: base(0, 2)
		{
		}

		public override bool IgnoreDuringCodeGeneration { get { return true; } }
	}
}
