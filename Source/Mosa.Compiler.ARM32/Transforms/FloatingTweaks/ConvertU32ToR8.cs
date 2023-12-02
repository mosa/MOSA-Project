// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// ConvertU32ToR8
/// </summary>
public sealed class ConvertU32ToR8 : BaseTransform
{
	public ConvertU32ToR8() : base(IR.ConvertU32ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IR.ConvertI32ToR8, result, operand1);
	}
}
