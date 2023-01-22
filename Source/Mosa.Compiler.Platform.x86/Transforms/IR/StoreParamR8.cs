// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreParamR8
	/// </summary>
	public sealed class StoreParamR8 : BaseTransform
	{
		public StoreParamR8() : base(IRInstruction.StoreParamR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand2 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand2);

			context.SetInstruction(X86.MovsdStore, null, transform.StackFrame, operand1, operand2);
		}
	}
}
