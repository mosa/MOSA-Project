// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR4ToI64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertR4ToI64 : BaseIRTransform
{
	public ConvertR4ToI64() : base(IR.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Cvttss2si64);
	}
}
