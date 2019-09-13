// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class AddCarryOut32CarryNotUsed : BaseTransformation
	{
		public AddCarryOut32CarryNotUsed() : base(IRInstruction.AddCarryOut32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return (context.Result2.Uses.Count == 0);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Add32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
