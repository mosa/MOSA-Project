
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// DivR4
	/// </summary>
	public sealed class DivR4 : BaseTransformation
	{
		public DivR4() : base(IRInstruction.DivR4, TransformationType.Manual | TransformationType.Transform)
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

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(context, operand1, transform);
			operand2 = X86TransformHelper.MoveConstantToFloatRegister(context, operand2, transform);

			context.SetInstruction(X86.Divss, result, operand1, operand2);
		}
	}
}
