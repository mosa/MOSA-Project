
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// MulOverflowOut32
	/// </summary>
	public sealed class MulOverflowOut32 : BaseTransformation
	{
		public MulOverflowOut32() : base(IRInstruction.MulOverflowOut32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var result2 = context.Result2;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = transform.AllocateVirtualRegister32();

			context.SetInstruction(X86.IMul32, result, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.Overflow, v1);
			context.AppendInstruction(X86.Movzx8To32, result2, v1);
		}
	}
}
