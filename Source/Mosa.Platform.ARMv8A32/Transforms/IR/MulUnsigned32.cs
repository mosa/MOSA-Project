// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// MulUnsigned32
	/// </summary>
	public sealed class MulUnsigned32 : BaseTransform
	{
		public MulUnsigned32() : base(IRInstruction.MulUnsigned32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveConstantRight(context);

			ARMv8A32TransformHelper.Translate(transform, context, ARMv8A32.Mul, false);
		}
	}
}
