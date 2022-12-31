// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv8A32.Transforms.FixedRegisters;
using Mosa.Platform.ARMv8A32.Transforms.Stack;
using Mosa.Platform.ARMv8A32.Transforms.Tweak;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Platform Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PlatformStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "ARMv8A32." + GetType().Name;

		public PlatformStage()
			: base(true, false, 0)
		{
			AddTranformations(TweakTransforms.List);
			AddTranformations(FixedRegistersTransforms.List);
			AddTranformations(StackTransforms.List);
			//AddTranformations(SpecialTransforms.List);
		}
	}
}
