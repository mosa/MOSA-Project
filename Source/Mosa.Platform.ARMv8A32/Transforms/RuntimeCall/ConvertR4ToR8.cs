// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR4ToR8
	/// </summary>
	public sealed class ConvertR4ToR8 : BaseTransform
	{
		public ConvertR4ToR8() : base(IRInstruction.ConvertR4ToR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math.FloatingPoint", "FloatToDouble");
		}
	}
}
