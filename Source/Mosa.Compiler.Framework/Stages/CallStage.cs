// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Call;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Call Stage
/// </summary>
public class CallStage : BaseTransformStage
{
	public CallStage()
		: base()
	{
		AddTranforms(CallTransforms.List);
	}
}
