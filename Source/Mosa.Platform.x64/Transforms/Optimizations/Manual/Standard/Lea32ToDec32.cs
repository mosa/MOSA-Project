// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Standard
{
	public sealed class Lea32ToDec32 : BaseTransformation
	{
		public Lea32ToDec32() : base(X64.Lea32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantSigned64 != -1)
				return false;

			if (context.Operand1 != context.Result)
				return false;

			if (context.Operand1.Register == CPURegister.RSP)
				return false;

			if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false) == TriState.No))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			context.SetInstruction(X64.Dec32, result, result);
		}
	}
}
