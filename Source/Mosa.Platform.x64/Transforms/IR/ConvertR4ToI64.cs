// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// ConvertR4ToI64
/// </summary>
public sealed class ConvertR4ToI64 : BaseIRTransform
{
	public ConvertR4ToI64() : base(IRInstruction.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Cvtss2sd);
	}
}
