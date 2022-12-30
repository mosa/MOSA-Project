// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.Optimizations.Auto;
using Mosa.Platform.x86.Transforms.Optimizations.Manual;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class OptimizationStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "x86." + GetType().Name;

		public OptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);
			AddTranformations(ManualTransforms.List);
		}
	}
}
