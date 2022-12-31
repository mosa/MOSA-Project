// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// IfThenElse32
	/// </summary>
	public sealed class IfThenElse32 : BaseTransform
	{
		public IfThenElse32() : base(IRInstruction.IfThenElse32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			if (operand2.IsConstant && operand3.IsConstant)
			{
				var v1 = transform.AllocateVirtualRegister(result.Type);

				context.SetInstruction(X86.Cmp32, null, operand1, transform.ConstantZero32);
				context.AppendInstruction(X86.Mov32, result, operand2);                                     // true
				context.AppendInstruction(X86.Mov32, v1, operand3);                                         // true
				context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, v1);             // false
			}
			else if (operand2.IsConstant && !operand3.IsConstant)
			{
				context.SetInstruction(X86.Cmp32, null, operand1, transform.ConstantZero32);
				context.AppendInstruction(X86.Mov32, result, operand2);                                 // true
				context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, operand3);       // false
			}
			else if (!operand2.IsConstant && operand3.IsConstant)
			{
				context.SetInstruction(X86.Cmp32, null, operand1, transform.ConstantZero32);
				context.AppendInstruction(X86.Mov32, result, operand3);                                     // true
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, operand2);    // false
			}
			else if (!operand2.IsConstant && !operand3.IsConstant)
			{
				context.SetInstruction(X86.Cmp32, null, operand1, transform.ConstantZero32);
				context.AppendInstruction(X86.Mov32, result, operand2);                                     // true
				context.AppendInstruction(X86.CMov32, ConditionCode.Equal, result, result, operand3);       // false
			}
		}
	}
}
