// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.IR;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Optimization Stage
/// </summary>
public class IRStage : BaseTransformStage
{
	public IRStage()
		: base(true, false)
	{
		AddTranformations(IRTransforms.List);
	}
}
