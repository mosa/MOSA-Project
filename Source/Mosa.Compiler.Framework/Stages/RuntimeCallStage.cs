// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Transforms.RuntimeCall;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage converts high level IR instructions to VM Calls
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class RuntimeCallStage : BaseTransformStage
{
	public RuntimeCallStage()
		: base(true, false, 1)
	{
		AddTranformations(StaticLoadTransforms.List);
	}
}
