// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv8A32.Transforms.RuntimeCall;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Runtime Call Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class RuntimeCallStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "ARMv8A32." + GetType().Name;

		public RuntimeCallStage()
			: base(true, false, 1)
		{
			AddTranformations(RuntimeCallTransforms.List);
		}
	}
}
