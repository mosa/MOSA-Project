
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamObject
	/// </summary>
	public sealed class LoadParamObject : BaseTransformation
	{
		public LoadParamObject() : base(IRInstruction.LoadParamObject, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovLoad32, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
