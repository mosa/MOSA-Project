// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms
{
	public abstract class BaseX86Transform : BaseTransform
	{
		public BaseX86Transform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

		public static Operand MoveConstantToFloatRegister(TransformContext transform, Context context, Operand operand)
		{
			if (!operand.IsConstant)
				return operand;

			var label = transform.CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? transform.VirtualRegisters.AllocateR4() : transform.VirtualRegisters.AllocateR8();

			var instruction = operand.IsR4 ? X86.MovssLoad : X86.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, Operand.Constant32_0);

			return v1;
		}

		public static Operand MoveConstantToFloatRegister(TransformContext transform, Context context, float value)
		{
			var label = transform.CreateR4Label(value);

			var v1 = transform.VirtualRegisters.AllocateR4();

			context.InsertBefore().SetInstruction(X86.MovssLoad, v1, label, Operand.Constant32_0);

			return v1;
		}

		public static Operand MoveConstantToFloatRegister(TransformContext transform, Context context, double value)
		{
			var label = transform.CreateR8Label(value);

			var v1 = transform.VirtualRegisters.AllocateR8();

			context.InsertBefore().SetInstruction(X86.MovsdLoad, v1, label, Operand.Constant32_0);

			return v1;
		}

		#endregion Helpers
	}
}
