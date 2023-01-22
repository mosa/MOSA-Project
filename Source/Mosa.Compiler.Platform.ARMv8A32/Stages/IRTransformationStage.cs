// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Platform.ARMv8A32.Transforms.IR;

namespace Mosa.Compiler.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// ARMv8A32 IR Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class IRTransformationStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "ARMv8A32." + GetType().Name;

		public IRTransformationStage()
			: base(true, false)
		{
			AddTranformations(IRTransforms.List);
		}
	}
}
