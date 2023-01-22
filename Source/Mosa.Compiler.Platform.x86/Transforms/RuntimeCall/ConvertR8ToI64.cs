// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR8ToI64
	/// </summary>
	public sealed class ConvertR8ToI64 : BaseTransform
	{
		public ConvertR8ToI64() : base(IRInstruction.ConvertR8ToI64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToI8");
		}
	}
}
