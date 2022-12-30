// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transforms;
using Mosa.Platform.x86.Transforms.Tweak;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class TweakStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "x86." + GetType().Name;

		public TweakStage()
			: base(true, false, 1)
		{
			AddTranformations(StackBuildTransforms.List);
		}
	}
}
