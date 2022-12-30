
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertI32ToR8
	/// </summary>
	public sealed class ConvertI32ToR8 : BaseTransformation
	{
		public ConvertI32ToR8() : base(IRInstruction.ConvertI32ToR8, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR8);

			context.ReplaceInstruction(X86.Cvtsi2sd32);
		}
	}
}
