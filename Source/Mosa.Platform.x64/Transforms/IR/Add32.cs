// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// Add32
	/// </summary>
	public sealed class Add32 : BaseTransform
	{
		public Add32() : base(IRInstruction.Add32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X64.Add32);
		}
	}
}
