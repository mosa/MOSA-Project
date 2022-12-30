
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadR8
	/// </summary>
	public sealed class LoadR8 : BaseTransform
	{
		public LoadR8() : base(IRInstruction.LoadR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X64.MovsdLoad, context.Result, context.Operand1, context.Operand2);
		}
	}
}
