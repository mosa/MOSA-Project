// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// ShiftLeft32
	/// </summary>
	public sealed class ShiftLeft32 : BaseTransform
	{
		public ShiftLeft32() : base(IRInstruction.ShiftLeft32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.Translate(transform, context, ARMv8A32.Lsl, true);
		}
	}
}
