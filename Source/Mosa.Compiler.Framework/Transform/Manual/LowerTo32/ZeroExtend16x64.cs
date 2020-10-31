// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class ZeroExtend16x64 : BaseTransformation
	{
		public ZeroExtend16x64() : base(IRInstruction.ZeroExtend16x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = transformContext.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.ZeroExtend16x32, v1, operand1);
			context.AppendInstruction(IRInstruction.To64, result, operand1, transformContext.ConstantZero32);
		}
	}
}
