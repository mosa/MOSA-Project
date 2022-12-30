// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.FinalTweak;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class FinalTweakStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "x86." + GetType().Name;

		public FinalTweakStage()
			: base(true, false, 1)
		{
			AddTranformations(FixedRegistersTransforms.List);
		}
	}
}
