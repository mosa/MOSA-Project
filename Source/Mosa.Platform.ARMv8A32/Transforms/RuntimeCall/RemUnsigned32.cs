// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// RemUnsigned32
	/// </summary>
	public sealed class RemUnsigned32 : BaseTransform
	{
		public RemUnsigned32() : base(IRInstruction.RemUnsigned32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "umod32");
		}
	}
}
