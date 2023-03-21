// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.CheckedConversion;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Checked Conversion Stage
/// </summary>
public class CheckedConversionStage : BaseTransformStage
{
	public CheckedConversionStage()
		: base(true, false)
	{
		AddTranforms(CheckedConversionTransforms.List);
	}
}
