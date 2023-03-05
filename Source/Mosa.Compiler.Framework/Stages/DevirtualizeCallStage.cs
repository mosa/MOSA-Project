// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Devirtualize;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Devirtualize Call Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public sealed class DevirtualizeCallStage : BaseTransformStage
{
	public DevirtualizeCallStage()
		: base(true, false, 1)
	{
		AddTranformations(DevirtualizeTransforms.List);
	}
}
