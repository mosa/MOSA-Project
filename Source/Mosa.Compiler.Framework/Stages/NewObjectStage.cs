﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Transforms.VM;

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
		AddTranformations(RuntimeCallTransforms.NewList);
	}
}
