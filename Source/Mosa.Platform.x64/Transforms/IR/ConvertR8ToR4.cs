// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ConvertR8ToR4
	/// </summary>
	public sealed class ConvertR8ToR4 : BaseTransform
	{
		public ConvertR8ToR4() : base(IRInstruction.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsFloatingPoint && !context.Result.IsFloatingPoint);

			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = X64TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);

			context.SetInstruction(X64.Cvtsd2ss, result, operand1);
		}
	}
}
