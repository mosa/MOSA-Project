// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.RuntimeTime;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// New Object Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class NewObjectStage : BaseTransformStage
{
	public NewObjectStage()
		: base(true, false, 1)
	{
		AddTranforms(RuntimeTimeTransforms.NewList);
	}
}
