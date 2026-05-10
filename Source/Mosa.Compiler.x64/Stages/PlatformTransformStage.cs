// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x64.Transforms.AddressMode;
using Mosa.Compiler.x64.Transforms.FixedRegisters;
using Mosa.Compiler.x64.Transforms.Optimizations.Manual.Special;
using Mosa.Compiler.x64.Transforms.Stack;
using Mosa.Compiler.x64.Transforms.Tweak;

namespace Mosa.Compiler.x64.Stages;

/// <summary>
/// Platform Transform Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x64." + GetType().Name;

	public PlatformTransformStage()
		: base(0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		AddTranforms(AddressModeTransforms.List);
		AddTranforms(SpecialTransforms.List);

		AddTranform(Mov32Unless.Instance);
		AddTranform(Mov64Unless.Instance);
		AddTranform(Mov32Coalescing.Instance);
		AddTranform(Mov64Coalescing.Instance);
		AddTranform(Deadcode.Instance);
	}
}
