// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// ConvertR4ToU32
/// </summary>
public sealed class ConvertR4ToU32 : BaseTransform
{
	public ConvertR4ToU32() : base(IR.ConvertR4ToU32, TransformType.Manual | TransformType.Transform)
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

		context.SetInstruction(IR.ConvertR4ToI32, result, operand1);
	}
}
