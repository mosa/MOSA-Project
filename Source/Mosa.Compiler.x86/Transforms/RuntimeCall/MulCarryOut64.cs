// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// MulCarryOut64
/// </summary>
[Transform("x86.RuntimeCall")]
public sealed class MulCarryOut64 : BaseTransform
{
	public MulCarryOut64() : base(Framework.IR.MulCarryOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -100;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = transform.GetMethod("Mosa.Runtime.Math.Multiplication", "Mul64Carry");

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;
		var result2 = context.Result2;

		var v1 = transform.LocalStack.Allocate(result2);   // REVIEW
		var v2 = transform.VirtualRegisters.Allocate32();

		Debug.Assert(method != null, $"Cannot find method: Mul64Carry");

		var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(Framework.IR.AddressOf, v2, v1);
		context.AppendInstruction(Framework.IR.CallStatic, result, symbol, operand1, operand2, v2);
		context.AppendInstruction(Framework.IR.LoadZeroExtend8x32, result2, v2, Operand.Constant32_0);

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
