// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Plug;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Replaces methods with their plugged implementation methods
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public class PlugStage : BaseTransformStage
{
	public PlugStage()
		: base()
	{
		AddTranforms(PlugTransforms.List);
	}
}
