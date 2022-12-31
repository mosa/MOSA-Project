// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// LoadParamSignExtend16x32
	/// </summary>
	public sealed class LoadParamSignExtend16x32 : BaseTransform
	{
		public LoadParamSignExtend16x32() : base(IRInstruction.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Transform)
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

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.LdrS16, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
