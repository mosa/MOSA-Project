// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding
{
	/// <summary>
	/// LoadSignExtend16x64FoldSub32
	/// </summary>
	public sealed class LoadSignExtend16x64FoldSub32 : BaseTransformation
	{
		public LoadSignExtend16x64FoldSub32() : base(IRInstruction.LoadSignExtend16x64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Sub32)
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transform.CreateConstant(Sub32(To32(t3), To32(t2)));

			context.SetInstruction(IRInstruction.LoadSignExtend16x64, result, t1, e1);
		}
	}
}
