// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transforms.Stack;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Runtime Call Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class BuildStackStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "x86." + GetType().Name;

		public BuildStackStage()
			: base(true, false, 1)
		{
			AddTranformations(StackTransforms.List);
		}
	}
}
