// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SubSameConstant : BaseTransformation
	{
		private static List<BaseInstruction> SubInstructions = new List<BaseInstruction>() { IRInstruction.Sub32, IRInstruction.Sub64, IRInstruction.SubFloatR4, IRInstruction.SubFloatR8 };

		public SubSameConstant() : base(SubInstructions, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Operand1.ConstantUnsignedLongInteger == context.Operand2.ConstantUnsignedLongInteger;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.CreateConstant(0);
		}
	}
}
