// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// StoreParam64
	/// </summary>
	public sealed class StoreParam64 : BaseTransform
	{
		public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);
			transform.SplitLongOperand(context.Operand2, out var valueLow, out var valueHigh);

			ARMv8A32TransformHelper.TransformStore(transform, context, ARMv8A32.Str32, transform.StackFrame, lowOffset, valueLow);
			ARMv8A32TransformHelper.TransformStore(transform, context.InsertAfter(), ARMv8A32.Str32, transform.StackFrame, highOffset, valueHigh);
		}
	}
}
