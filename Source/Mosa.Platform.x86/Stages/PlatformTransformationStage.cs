// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.AddressMode;
using Mosa.Platform.x86.Transforms.FixedRegisters;
using Mosa.Platform.x86.Transforms.Optimizations.Manual.Special;
using Mosa.Platform.x86.Transforms.Stack;
using Mosa.Platform.x86.Transforms.Tweak;

namespace Mosa.Platform.x86.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public PlatformTransformationStage()
		: base(true, false, 0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		AddTranforms(AddressModeTransforms.List);

		//AddTranform(new GetLow32Register());

		AddTranform(new Mov32Unless());
		AddTranform(new Mov32Coalescing());
		AddTranform(new Deadcode());
	}
}
