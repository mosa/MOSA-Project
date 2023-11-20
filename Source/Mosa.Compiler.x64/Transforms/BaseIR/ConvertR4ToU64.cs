// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR4ToU64
/// </summary>
public sealed class ConvertR4ToU64 : BaseIRTransform
{
	public ConvertR4ToU64() : base(IR.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//context.ReplaceInstruction(X64.Cvttss2si64); //
	}
}
