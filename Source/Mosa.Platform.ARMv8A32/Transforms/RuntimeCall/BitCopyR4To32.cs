// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// BitCopyR4To32
	/// </summary>
	public sealed class BitCopyR4To32 : BaseTransform
	{
		public BitCopyR4To32() : base(IRInstruction.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math.FloatingPoint", "BitCopyFloatR4ToInt32");
		}
	}
}
