// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
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
			var offset = context.Operand1;
			var value = context.Operand2;

			var valueLow = transformContext.AllocateVirtualRegister32();
			var valueHigh = transformContext.AllocateVirtualRegister32();

			transformContext.SplitLongOperand(offset, out Operand op1Low, out Operand op1High);

			context.SetInstruction(IRInstruction.GetLow32, valueLow, value);
			context.AppendInstruction(IRInstruction.GetHigh32, valueHigh, value);

			context.AppendInstruction(IRInstruction.StoreParam32, null, op1Low, valueLow);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1High, valueHigh);
		}
	}
}
