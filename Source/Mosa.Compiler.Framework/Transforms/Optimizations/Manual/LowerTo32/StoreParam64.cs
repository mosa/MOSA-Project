// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class StoreParam64 : BaseTransform
	{
		public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return transform.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var offset = context.Operand1;
			var value = context.Operand2;

			var valueLow = transform.AllocateVirtualRegister32();
			var valueHigh = transform.AllocateVirtualRegister32();

			transform.SplitLongOperand(offset, out Operand op1Low, out Operand op1High);

			context.SetInstruction(IRInstruction.GetLow32, valueLow, value);
			context.AppendInstruction(IRInstruction.GetHigh32, valueHigh, value);

			context.AppendInstruction(IRInstruction.StoreParam32, null, op1Low, valueLow);
			context.AppendInstruction(IRInstruction.StoreParam32, null, op1High, valueHigh);
		}
	}
}
