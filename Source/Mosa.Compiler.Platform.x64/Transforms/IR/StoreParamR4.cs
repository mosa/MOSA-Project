// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// StoreParamR4
	/// </summary>
	public sealed class StoreParamR4 : BaseTransform
	{
		public StoreParamR4() : base(IRInstruction.StoreParamR4, TransformType.Manual | TransformType.Transform)
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

			operand2 = X64TransformHelper.MoveConstantToFloatRegister(transform, context, operand2);

			context.SetInstruction(X64.MovssStore, null, transform.StackFrame, operand1, operand2);
		}
	}
}
