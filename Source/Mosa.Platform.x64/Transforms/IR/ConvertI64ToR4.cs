// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// ConvertI64ToR4
/// </summary>
public sealed class ConvertI64ToR4 : BaseIRTransform
{
	public ConvertI64ToR4() : base(IRInstruction.ConvertI64ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.Cvtsi2ss64, context.Result, context.Operand1);
	}
}
