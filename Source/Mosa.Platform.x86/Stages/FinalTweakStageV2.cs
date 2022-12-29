// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transform.FinalTweak;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class FinalTweakStageV2 : BaseOptimizationStage
	{
		public override string Name => "x86." + GetType().Name;

		public FinalTweakStageV2()
			: base(true, false, 1)
		{
			AddTranformations(Transforms.FinalTweakList);
		}
	}
}
