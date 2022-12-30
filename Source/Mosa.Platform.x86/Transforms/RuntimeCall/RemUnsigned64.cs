// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// RemUnsigned64
	/// </summary>
	public sealed class RemUnsigned64 : BaseTransformation
	{
		public RemUnsigned64() : base(IRInstruction.RemUnsigned64, TransformationType.Manual | TransformationType.Transform)
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
