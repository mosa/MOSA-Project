
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// SubR4
	/// </summary>
	public sealed class SubR4 : BaseTransformation
	{
		public SubR4() : base(IRInstruction.SubR4, TransformationType.Manual | TransformationType.Transform)
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

			context.SetInstruction(X86.Subss, result, operand1, operand2);
		}
	}
}
