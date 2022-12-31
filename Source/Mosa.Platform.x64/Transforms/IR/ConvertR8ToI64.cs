// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
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
			Debug.Assert(context.Result.IsInteger && !context.Result.IsFloatingPoint);
			context.ReplaceInstruction(X64.Cvttss2si64);
		}
	}
}
