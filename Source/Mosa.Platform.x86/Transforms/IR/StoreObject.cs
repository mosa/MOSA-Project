
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreObject
	/// </summary>
	public sealed class StoreObject : BaseTransformation
	{
		public StoreObject() : base(IRInstruction.StoreObject, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}
