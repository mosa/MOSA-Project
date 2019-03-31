// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Floating Point Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.Intel.Stages.FloatingPointStage" />
	public sealed class FloatingPointStage : Intel.Stages.FloatingPointStage
	{
		protected override bool IsLoad(BaseInstruction instruction)
		{
			return instruction == X64.MovsdLoad || instruction == X64.MovssLoad || instruction == X64.MovLoad32 || instruction == X64.MovLoad64;
		}

		protected override bool IsIntegerToFloating(BaseInstruction instruction)
		{
			return instruction == X64.Cvtsi2sd64 || instruction == X64.Cvtsi2ss64 ||
				   instruction == X64.Cvtsi2sd32 || instruction == X64.Cvtsi2ss32;
		}

		protected override BaseInstruction MovssLoad { get { return X64.MovssLoad; } }

		protected override BaseInstruction MovsdLoad { get { return X64.MovsdLoad; } }
	}
}
