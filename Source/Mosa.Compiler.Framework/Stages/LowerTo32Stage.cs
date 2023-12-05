// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.LowerTo32;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	LowerTo32 Stage
/// </summary>
public class LowerTo32Stage : BaseTransformStage
{
	public LowerTo32Stage()
		: base()
	{
		AddTranforms(LowerTo32Transforms.List);

		EnableBlockOptimizations = false;
	}

	protected override void Setup()
	{
		Transform.SetStageOption(TransformStageOption.LowerTo32);
	}

	protected override bool SetupPhase(int phase)
	{
		return phase == 0;
	}
}
