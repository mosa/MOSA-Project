// Copyright (c) MOSA Project. Licensed under the New BSD License.


using Mosa.Platform.x86.Transforms.IR;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 IR Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class IRTransformationStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "x86." + GetType().Name;

		public IRTransformationStage()
			: base(true, false)
		{
			AddTranformations(IRTransforms.List);
		}
	}
}
