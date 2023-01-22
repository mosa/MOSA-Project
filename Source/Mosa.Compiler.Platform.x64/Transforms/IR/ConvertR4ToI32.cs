// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ConvertR4ToI32
	/// </summary>
	public sealed class ConvertR4ToI32 : BaseTransform
	{
		public ConvertR4ToI32() : base(IRInstruction.ConvertR4ToI32, TransformType.Manual | TransformType.Transform)
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

			operand1 = X64TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);

			context.SetInstruction(X64.Cvttss2si32, result, operand1);
		}
	}
}
