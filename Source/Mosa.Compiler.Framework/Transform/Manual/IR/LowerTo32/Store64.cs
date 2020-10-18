// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class Store64 : BaseTransformation
	{
		public Store64() : base(IRInstruction.Store64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var address = context.Operand1;
			var offset = context.Operand2;
			var value = context.Operand3;

			var valueLow = transformContext.AllocateVirtualRegister32();
			var valueHigh = transformContext.AllocateVirtualRegister32();
			var offsetLow = transformContext.AllocateVirtualRegister32();
			var addressLow = transformContext.AllocateVirtualRegister32();
			var offset4 = transformContext.AllocateVirtualRegister32();

			transformContext.SetGetLow64(context, valueLow, value);
			transformContext.AppendGetHigh64(context, valueHigh, value);
			transformContext.AppendGetLow64(context, addressLow, address);
			transformContext.AppendGetLow64(context, offsetLow, offset);
			context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset, valueLow);
			context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, transformContext.CreateConstant((uint)4));
			context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset4, valueHigh);
		}
	}
}
