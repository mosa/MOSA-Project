// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// AddCarryIn32
/// </summary>
public sealed class AddCarryIn32 : BaseIRTransform
{
	public AddCarryIn32() : base(IRInstruction.AddCarryIn32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		var v1 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X64.Bt32, v1, operand3, transform.Constant64_0);
		context.AppendInstruction(X64.Adc32, result, operand1, operand2);
	}
}
