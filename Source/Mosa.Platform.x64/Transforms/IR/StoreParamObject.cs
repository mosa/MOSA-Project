
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// StoreParamObject
	/// </summary>
	public sealed class StoreParamObject : BaseTransform
	{
		public StoreParamObject() : base(IRInstruction.StoreParamObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovStore64, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
