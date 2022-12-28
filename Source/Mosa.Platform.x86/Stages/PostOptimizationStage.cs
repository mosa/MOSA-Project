// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transform.Auto;
using Mosa.Platform.x86.Transform.Manual;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseOptimizationStage
	{
		public override string Name => "x86." + GetType().Name;

		public PostOptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);
			AddTranformations(ManualTransforms.List);
			AddTranformations(ManualTransforms.PostList);
		}
	}
}
