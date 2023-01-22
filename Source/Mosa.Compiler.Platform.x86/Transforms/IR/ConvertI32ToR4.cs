// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertI32ToR4
	/// </summary>
	public sealed class ConvertI32ToR4 : BaseTransform
	{
		public ConvertI32ToR4() : base(IRInstruction.ConvertI32ToR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR4);

			context.ReplaceInstruction(X86.Cvtsi2ss32);
		}
	}
}
