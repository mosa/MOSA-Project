// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.PlatformIntrinsic;

/// <summary>
/// IntrinsicMethodCall
/// </summary>
public sealed class IntrinsicMethodCall : BaseTransform
{
	public IntrinsicMethodCall() : base(IR.IntrinsicMethodCall, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return transform.Architecture.GetInstrinsicMethod(context.Operand1.Method.ExternMethodModule) != null;
	}

	public override void Transform(Context context, Transform transform)
	{
		var intrinsic = transform.Architecture.GetInstrinsicMethod(context.Operand1.Method.ExternMethodModule);

		var operands = context.GetOperands();
		operands.RemoveAt(0);

		context.SetInstruction(IR.IntrinsicMethodCall, context.Result, operands);

		intrinsic(context, transform);
	}
}
