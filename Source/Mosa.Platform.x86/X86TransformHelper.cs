// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{
	public static class X86TransformHelper
	{
		public static Operand MoveConstantToFloatRegister(Context context, Operand operand, TransformContext transform)
		{
			if (!operand.IsConstant)
				return operand;

			var label = transform.CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? transform.AllocateVirtualRegisterR4() : transform.AllocateVirtualRegisterR8();

			var instruction = operand.IsR4 ? (BaseInstruction)X86.MovssLoad : X86.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, transform.ConstantZero32);

			return v1;
		}

		//private void Transform(Context context, TransformContext transform) // scratch code
		//{
		//	var result = context.Result;
		//	var operand1 = context.Operand1;

		//	if (operand1.IsConstant)
		//	{
		//		result = transform.LoadValueR4(context, operand1.ConstantFloat, X86.MovssLoad);
		//	}

		//	context.SetInstruction(X86.Movdssi32, result, operand1);
		//}
	}
}
