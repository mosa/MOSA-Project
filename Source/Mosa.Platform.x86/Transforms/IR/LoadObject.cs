
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadObject
	/// </summary>
	public sealed class LoadObject : BaseTransformation
	{
		public LoadObject() : base(IRInstruction.LoadObject, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			transform.OrderOperands(context);

			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
