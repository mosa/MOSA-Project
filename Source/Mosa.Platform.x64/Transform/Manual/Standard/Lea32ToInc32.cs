// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;


namespace Mosa.Platform.x64.Transform.Manual.Standard
{
	public sealed class Lea32ToInc32 : BaseTransformation
	{
		public Lea32ToInc32() : base(X64.Lea32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 1)
				return false;

			if (context.Operand1 != context.Result)
				return false;

			if (context.Operand1.Register == CPURegister.RSP)
				return false;

			if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false) == TriState.No))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			context.SetInstruction(X64.Inc32, result, result);
		}
	}
}
