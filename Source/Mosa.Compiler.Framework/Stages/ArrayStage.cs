// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Array;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Optimization Stage
/// </summary>
public class ArrayStage : BaseTransformStage
{
	public ArrayStage()
		: base(true, false)
	{
		AddTranformations(ArrayTransforms.List);
	}
}
