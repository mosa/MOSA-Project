// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x64.Transforms.IR;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// x64 IR Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class IRTransformationStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "x64." + GetType().Name;

		public IRTransformationStage()
			: base(true, false)
		{
			AddTranformations(IRTransforms.List);
		}
	}
}
