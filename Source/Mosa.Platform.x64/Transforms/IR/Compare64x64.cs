
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// Compare64x64
	/// </summary>
	public sealed class Compare64x64 : BaseTransform
	{
		public Compare64x64() : base(IRInstruction.Compare64x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var condition = context.ConditionCode;
			var resultOperand = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = transform.AllocateVirtualRegister32();
			context.SetInstruction(X64.Cmp64, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, condition, v1);
			context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
		}
	}
}
