// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Floating Point Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.Intel.Stages.FloatingPointStage" />
	public sealed class FloatingPointStage : Intel.Stages.FloatingPointStage
	{
		protected override bool IsLoad(BaseInstruction instruction)
		{
			return instruction == X86.MovsdLoad || instruction == X86.MovssLoad || instruction == X86.MovLoad32;
		}

		protected override bool IsIntegerToFloating(BaseInstruction instruction)
		{
			return instruction == X86.Cvtsi2sd32 || instruction == X86.Cvtsi2ss32;
		}

		protected override BaseInstruction MovssLoad { get { return X86.MovssLoad; } }

		protected override BaseInstruction MovsdLoad { get { return X86.MovsdLoad; } }
	}
}
