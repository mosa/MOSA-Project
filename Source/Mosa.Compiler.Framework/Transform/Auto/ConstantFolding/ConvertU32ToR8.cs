// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantFolding
{
	/// <summary>
	/// ConvertU32ToR8
	/// </summary>
	public sealed class ConvertU32ToR8 : BaseTransformation
	{
		public ConvertU32ToR8() : base(IRInstruction.ConvertI32ToR8, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			var e1 = transformContext.CreateConstant(ToR8(To32(t1)));

			context.SetInstruction(IRInstruction.MoveR8, result, e1);
		}
	}
}
