// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x64.Transform.Auto;
using Mosa.Platform.x64.Transform.Manual;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class EarlyOptimizationStage : BaseOptimizationStage
	{
		public override string Name => "x64." + GetType().Name;

		public EarlyOptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);
			AddTranformations(ManualTransforms.List);
			AddTranformations(ManualTransforms.EarlyList);
		}
	}
}
