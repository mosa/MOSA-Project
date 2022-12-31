// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// IfThenElse64
	/// </summary>
	public sealed class IfThenElse64 : BaseTransform
	{
		public IfThenElse64() : base(IRInstruction.IfThenElse64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Operand1;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.SetInstruction(X64.Cmp64, null, operand1, transform.ConstantZero64);
			context.AppendInstruction(X64.CMov64, ConditionCode.NotEqual, result, result, operand1);    // true
			context.AppendInstruction(X64.CMov64, ConditionCode.Equal, result, result, operand2);       // false
		}
	}
}
