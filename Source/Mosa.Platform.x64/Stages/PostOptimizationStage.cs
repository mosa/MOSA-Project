// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x64.Transforms.Optimizations.Auto;
using Mosa.Platform.x64.Transforms.Optimizations.Manual;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// X64 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseOptimizationStage
	{
		public override string Name => "x64." + GetType().Name;

		public PostOptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);
			AddTranformations(ManualTransforms.List);
			AddTranformations(ManualTransforms.PostList);
		}
	}
}
