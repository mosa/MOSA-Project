// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreObject
	/// </summary>
	public sealed class StoreObject : BaseTransform
	{
		public StoreObject() : base(IRInstruction.StoreObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderLoadStoreOperands(context);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}
