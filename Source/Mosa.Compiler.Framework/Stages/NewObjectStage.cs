// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.VM;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// New Object Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
public sealed class NewObjectStage : BaseTransformStage
{
	public NewObjectStage()
		: base(true, false, 1)
	{
		AddTranformations(VMTransforms.NewList);
	}
}
