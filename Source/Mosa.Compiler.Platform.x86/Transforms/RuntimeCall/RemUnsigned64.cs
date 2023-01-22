// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// RemUnsigned64
	/// </summary>
	public sealed class RemUnsigned64 : BaseTransform
	{
		public RemUnsigned64() : base(IRInstruction.RemUnsigned64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "umod64");
		}
	}
}
