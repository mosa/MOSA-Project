// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class Load64 : BaseTransformation
	{
		public Load64() : base(IRInstruction.Load64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var address = context.Operand1;
			var offset = context.Operand2;

			var resultLow = transformContext.AllocateVirtualRegister32();
			var resultHigh = transformContext.AllocateVirtualRegister32();
			var offsetLow = transformContext.AllocateVirtualRegister32();
			var addressLow = transformContext.AllocateVirtualRegister32();
			var offset4 = transformContext.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, addressLow, address);
			context.AppendInstruction(IRInstruction.GetLow32, offsetLow, offset);

			context.AppendInstruction(IRInstruction.Load32, resultLow, addressLow, offset);
			context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, transformContext.CreateConstant((uint)4));
			context.AppendInstruction(IRInstruction.Load32, resultHigh, addressLow, offset4);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
