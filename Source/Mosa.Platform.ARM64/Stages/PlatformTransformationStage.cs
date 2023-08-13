// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARM64.Transforms.FixedRegisters;
using Mosa.Platform.ARM64.Transforms.Stack;
using Mosa.Platform.ARM64.Transforms.Tweak;

namespace Mosa.Platform.ARM64.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM64." + GetType().Name;

	public PlatformTransformationStage()
		: base(true, false, 0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		//AddTranformations(SpecialTransforms.List);
	}
}
