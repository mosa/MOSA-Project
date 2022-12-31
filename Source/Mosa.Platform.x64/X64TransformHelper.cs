// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64
{
	public static class X64TransformHelper
	{
		public static Operand MoveConstantToFloatRegister(Context context, Operand operand, TransformContext transform)
		{
			if (!operand.IsConstant)
				return operand;

			var label = transform.CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? transform.AllocateVirtualRegisterR4() : transform.AllocateVirtualRegisterR8();

			var instruction = operand.IsR4 ? (BaseInstruction)X64.MovssLoad : X64.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, transform.ConstantZero32);

			return v1;
		}

	}
}
