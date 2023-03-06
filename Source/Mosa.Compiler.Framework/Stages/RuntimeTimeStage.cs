// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.RuntimeTime;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage converts high level IR instructions to VM Calls
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class RuntimeTimeStage : BaseTransformStage
{
	public RuntimeTimeStage()
		: base(true, false, 1)
	{
		AddTranformations(RuntimeTimeTransforms.RuntimeList);
	}
}
