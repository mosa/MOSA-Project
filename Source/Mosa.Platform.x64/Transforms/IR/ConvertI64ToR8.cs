
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ConvertI64ToR8
	/// </summary>
	public sealed class ConvertI64ToR8 : BaseTransform
	{
		public ConvertI64ToR8() : base(IRInstruction.ConvertI64ToR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.Cvtsi2sd64, context.Result, context.Operand1);
		}
	}
}
