// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

namespace Mosa.Compiler.ARM32.Stages;

/// <summary>
/// ARM32 IR Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class FloatingTweakTransform : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM32." + GetType().Name;

	public FloatingTweakTransform()
		: base()
	{
		AddTranforms(FloatingTweakTransforms.List);
	}
}
