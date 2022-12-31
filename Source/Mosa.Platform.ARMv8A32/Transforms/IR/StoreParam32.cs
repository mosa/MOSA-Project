// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// StoreParam32
	/// </summary>
	public sealed class StoreParam32 : BaseTransform
	{
		public StoreParam32() : base(IRInstruction.StoreParam32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.TransformStore(transform, context, ARMv8A32.Str32, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
