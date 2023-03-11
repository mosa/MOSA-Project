// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Call;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Call Stage
/// </summary>
public class CallStage : BaseTransformStage
{
	public CallStage()
		: base(true, false)
	{
		AddTranforms(CallTransforms.List);
	}
}
