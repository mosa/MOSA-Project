
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadR4
	/// </summary>
	public sealed class LoadR4 : BaseTransform
	{
		public LoadR4() : base(IRInstruction.LoadR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X64.MovssLoad, context.Result, context.Operand1, context.Operand2);
		}
	}
}
