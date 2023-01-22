// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ConvertR4ToI64
	/// </summary>
	public sealed class ConvertR4ToI64 : BaseTransform
	{
		public ConvertR4ToI64() : base(IRInstruction.ConvertR4ToI64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X64.Cvtss2sd);
		}
	}
}
