// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net;

namespace Mosa.Compiler.Framework.Transforms.PlatformIntrinsic;

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

	public override void Transform(Context context, TransformContext transformContext)
	{
		var intrinsic = transformContext.Architecture.GetInstrinsicMethod(context.Operand1.Method.ExternMethodModule);

		var operands = context.GetOperands();
		operands.RemoveAt(0);

		context.SetInstruction(IRInstruction.IntrinsicMethodCall, context.Result, operands);

		intrinsic(context, transformContext);
	}
}
