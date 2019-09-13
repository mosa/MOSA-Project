// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.LowerTo32
{
	public sealed class Add64 : BaseTransformation
	{
		public Add64() : base(IRInstruction.Add64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var op0Low = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);
			var op0High = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);
			var op1Low = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);
			var op1High = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);
			var resultLow = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);
			var resultHigh = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.I4);

			var resultCarry = transformContext.AllocateVirtualRegister(transformContext.TypeSystem.BuiltIn.Boolean);

			transformContext.SetGetLow64(context, op0Low, operand1);
			transformContext.AppendGetHigh64(context, op0High, operand1);
			transformContext.AppendGetLow64(context, op1Low, operand2);
			transformContext.AppendGetHigh64(context, op1High, operand2);

			context.AppendInstruction2(IRInstruction.AddCarryOut32, resultLow, resultCarry, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.AddWithCarry32, resultHigh, op0High, op1High, resultCarry);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
