
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ConvertR8ToI32
	/// </summary>
	public sealed class ConvertR8ToI32 : BaseTransform
	{
		public ConvertR8ToI32() : base(IRInstruction.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
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

			operand1 = X64TransformHelper.MoveConstantToFloatRegister(context, operand1, transform);

			context.SetInstruction(X64.Cvttsd2si32, result, operand1);
		}
	}
}
