// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// ConvertR8ToU32
/// </summary>
public sealed class ConvertR8ToU32 : BaseTransform
{
	public ConvertR8ToU32() : base(IR.ConvertR8ToU32, TransformType.Manual | TransformType.Transform)
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

		context.SetInstruction(IR.ConvertR8ToI32, result, operand1);
	}
}
