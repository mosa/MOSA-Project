// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Auto.IR.ConstantFolding
{
	/// <summary>
	/// DivSigned32
	/// </summary>
	public sealed class DivSigned32 : BaseTransformation
	{
		public DivSigned32() : base(IRInstruction.DivSigned32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(DivSigned32(ToSigned32(t1), ToSigned32(t2)));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}
}
