// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv8A32.Transforms.Optimizations.Auto;
using Mosa.Platform.ARMv8A32.Transforms.Optimizations.Manual;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// ARMv8A32 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class OptimizationStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "ARMv8A32." + GetType().Name;

		public OptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);
			AddTranformations(ManualTransforms.List);
		}
	}
}
