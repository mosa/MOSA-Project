// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.CheckedConversion;
using Mosa.Compiler.Framework.Transforms.IR;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	IR Transform Stage
/// </summary>
public class IRTransformsStage : BaseTransformStage
{
	public IRTransformsStage()
		: base()
	{
		AddTranforms(IRTransforms.List);
		AddTranforms(CheckedConversionTransforms.List);
	}
}
