// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// LoadZeroExtend8x32
	/// </summary>
	public sealed class LoadZeroExtend8x32 : BaseTransform
	{
		public LoadZeroExtend8x32() : base(IRInstruction.LoadZeroExtend8x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			transform.OrderLoadStoreOperands(context);

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldr8, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
