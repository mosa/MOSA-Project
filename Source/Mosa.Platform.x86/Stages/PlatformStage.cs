// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.FixedRegisters;
using Mosa.Platform.x86.Transforms.Stack;
using Mosa.Platform.x86.Transforms.Tweak;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Platform Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class PlatformStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "x86." + GetType().Name;

		public PlatformStage()
			: base(true, false, 0)
		{
			AddTranformations(TweakTransforms.List);
			AddTranformations(FixedRegistersTransforms.List);
			AddTranformations(StackTransforms.List);
			AddTranformations(SpecialTransforms.List);
		}
	}
}
