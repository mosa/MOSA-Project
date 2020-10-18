// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class StoreParam64 : BaseTransformation
	{
		public StoreParam64() : base(IRInstruction.StoreParam64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = transformContext.AllocateVirtualRegister32();
			var op0High = transformContext.AllocateVirtualRegister32();

			transformContext.SplitLongOperand(operand1, out Operand op1Low, out Operand op1High);
			transformContext.SplitLongOperand(operand2, out Operand _, out Operand _);

			transformContext.SetGetLow64(context, op0Low, operand2);
			transformContext.AppendGetHigh64(context, op0High, operand2);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1Low, op0Low);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1High, op0High);
		}
	}
}
