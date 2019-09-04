﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class DivSigned32 : BaseTransformation
	{
		public DivSigned32() : base(IRInstruction.DivSigned32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToConstant(context, ((ulong)(context.Operand1.ConstantSigned64 / context.Operand2.ConstantSigned64)) & 0xFFFFFFFF);
		}
	}
}
