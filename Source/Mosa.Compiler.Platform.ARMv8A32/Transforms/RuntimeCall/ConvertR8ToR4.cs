// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR8ToR4
	/// </summary>
	public sealed class ConvertR8ToR4 : BaseTransform
	{
		public ConvertR8ToR4() : base(IRInstruction.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.ARMv8A32.Math.FloatingPoint", "DoubleToFloat");
		}
	}
}
