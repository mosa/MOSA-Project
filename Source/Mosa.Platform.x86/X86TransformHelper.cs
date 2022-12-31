// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86
{
	public static class X86TransformHelper
	{
		public static Operand MoveConstantToFloatRegister(TransformContext transform, Context context, Operand operand)
		{
			if (!operand.IsConstant)
				return operand;

			var label = transform.CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? transform.AllocateVirtualRegisterR4() : transform.AllocateVirtualRegisterR8();

			var instruction = operand.IsR4 ? (BaseInstruction)X86.MovssLoad : X86.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, transform.Constant32_0);

			return v1;
		}
	}
}
