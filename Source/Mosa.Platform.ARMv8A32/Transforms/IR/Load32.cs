// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// Load32
	/// </summary>
	public sealed class Load32 : BaseTransform
	{
		public Load32() : base(IRInstruction.Load32, TransformType.Manual | TransformType.Transform)
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

			transform.OrderOperands(context);

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldr32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
