
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertR8ToR4
	/// </summary>
	public sealed class ConvertR8ToR4 : BaseTransformation
	{
		public ConvertR8ToR4() : base(IRInstruction.ConvertR8ToR4, TransformationType.Manual | TransformationType.Transform)
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

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(context, operand1, transform);

			context.SetInstruction(X86.Cvtsd2ss, result, operand1);
		}
	}
}
