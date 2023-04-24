// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.RuntimeCall;

/// <summary>
/// MulOverflowOut64
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class MulOverflowOut64 : BaseTransform
{
	public MulOverflowOut64() : base(IRInstruction.MulOverflowOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var methodName = "Mul64Overflow";
		var method = transform.GetMethod("Mosa.Runtime.Math.Multiplication", methodName);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;
		var result2 = context.Result2;

		var v1 = transform.LocalStack.Allocate(result2);
		var v2 = transform.VirtualRegisters.Allocate32();

		Debug.Assert(method != null, $"Cannot find method: {methodName}");

		var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(IRInstruction.AddressOf, v2, v1);
		context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2, v2);
		context.AppendInstruction(IRInstruction.LoadZeroExtend8x32, result2, v2, Operand.Constant32_0);

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
