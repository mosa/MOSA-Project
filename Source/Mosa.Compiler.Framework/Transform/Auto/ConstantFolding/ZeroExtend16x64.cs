// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantFolding
{
	/// <summary>
	/// ZeroExtend16x64
	/// </summary>
	public sealed class ZeroExtend16x64 : BaseTransformation
	{
		public ZeroExtend16x64() : base(IRInstruction.ZeroExtend16x64, TransformationType.Auto| TransformationType.Optimization)
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

			var e1 = transformContext.CreateConstant(To64(ToShort(t1)));

			context.SetInstruction(IRInstruction.Move64, result, e1);
		}
	}
}
