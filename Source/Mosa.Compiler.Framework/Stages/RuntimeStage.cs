// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.CheckedConversion;
using Mosa.Compiler.Framework.Transforms.Runtime;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stage converts high level IR instructions to VM Calls
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class RuntimeStage : BaseTransformStage
{
	public RuntimeStage()
		: base(true, false, 1)
	{
		AddTranforms(RuntimeTransforms.RuntimeList);
	}
}
