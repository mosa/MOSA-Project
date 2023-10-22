// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ConvertR4ToU64
/// </summary>
[Transform("x64.IR")]
public sealed class ConvertR4ToU64 : BaseIRTransform
{
	public ConvertR4ToU64() : base(IRInstruction.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//context.ReplaceInstruction(X64.Cvttss2si64); //
	}
}
