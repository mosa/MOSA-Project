
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadR4
	/// </summary>
	public sealed class LoadR4 : BaseTransformation
	{
		public LoadR4() : base(IRInstruction.LoadR4, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Result, context.Operand1, context.Operand2);
		}
	}
}
