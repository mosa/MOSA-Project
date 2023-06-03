// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.CheckedConversion;
using Mosa.Compiler.Framework.Transforms.IR;
using Mosa.Compiler.Framework.Transforms.TruncateTo32;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	IR Transform Stage
/// </summary>
public class IRTransformsStage : BaseTransformStage
{
	public IRTransformsStage()
		: base(true, false)
	{
		AddTranforms(IRTransforms.List);
		AddTranforms(CheckedConversionTransforms.List);
		AddTranforms(TruncateTo32Transforms.List);
	}
}
