// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.PlatformIntrinsic;

/// <summary>
/// IntrinsicMethodCall
/// </summary>
public sealed class IntrinsicMethodCall : BaseTransform
{
	public IntrinsicMethodCall() : base(IRInstruction.IntrinsicMethodCall, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return transform.Architecture.GetInstrinsicMethod(context.Operand1.Method.ExternMethodModule) != null;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var intrinsic = transform.Architecture.GetInstrinsicMethod(context.Operand1.Method.ExternMethodModule);

		var operands = context.GetOperands();
		operands.RemoveAt(0);

		context.SetInstruction(IRInstruction.IntrinsicMethodCall, context.Result, operands);

		intrinsic(context, transform.MethodCompiler);
	}
}
