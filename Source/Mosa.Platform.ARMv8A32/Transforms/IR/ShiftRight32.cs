// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// ShiftRight32
	/// </summary>
	public sealed class ShiftRight32 : BaseTransform
	{
		public ShiftRight32() : base(IRInstruction.ShiftRight32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.Translate(transform, context, ARMv8A32.Lsr, true);
		}
	}
}
