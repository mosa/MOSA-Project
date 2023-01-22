// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR8ToU64
	/// </summary>
	public sealed class ConvertR8ToU64 : BaseTransform
	{
		public ConvertR8ToU64() : base(IRInstruction.ConvertR8ToU64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Conversion", "R8ToU8");
		}
	}
}
