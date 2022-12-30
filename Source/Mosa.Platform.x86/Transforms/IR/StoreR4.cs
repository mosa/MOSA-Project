
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreR4
	/// </summary>
	public sealed class StoreR4 : BaseTransformation
	{
		public StoreR4() : base(IRInstruction.StoreR4, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			operand3 = X86TransformHelper.MoveConstantToFloatRegister(context, operand3, transform);

			context.SetInstruction(X86.MovssStore, null, operand1, operand2, operand3);
		}
	}
}
