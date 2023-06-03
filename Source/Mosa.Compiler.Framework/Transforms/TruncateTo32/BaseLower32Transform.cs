// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.TruncateTo32
{
	public abstract class BaseTruncateTo32Transform : BaseTransform
	{
		public BaseTruncateTo32Transform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		//public override int Priority => 0;



		public void TruncateOperandTo32(Context context, TransformContext transform, int index)
		{
			if (context.GetOperand(index).IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand1);
				context.SetOperand(index, low);
			}
		}

		public void TruncateOperand1To32(Context context, TransformContext transform)
		{
			if (context.Operand1.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand1);
				context.Operand1 = low;
			}
		}

		public void TruncateOperand1And2To32(Context context, TransformContext transform)
		{
			if (context.Operand1.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand1);
				context.Operand1 = low;
			}
			if (context.Operand2.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand2);
				context.Operand2 = low;
			}
		}

		public void TruncateOperand1Thru3To32(Context context, TransformContext transform)
		{
			if (context.Operand1.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand1);
				context.Operand1 = low;
			}
			if (context.Operand2.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand2);
				context.Operand2 = low;
			}
			if (context.Operand3.IsInt64)
			{
				var low = transform.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, context.Operand3);
				context.Operand3 = low;
			}
		}
	}
}
