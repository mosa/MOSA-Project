// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Transform.Manual
{
	public sealed class Lea32ToDec32 : BaseTransformation
	{
		public Lea32ToDec32() : base(X86.Lea32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantSigned64 != -1)
				return false;

			if (context.Operand1 != context.Result)
				return false;

			if (context.Operand1.Register == GeneralPurposeRegister.ESP)
				return false;

			if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false) == TriState.No))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			context.SetInstruction(X86.Dec32, result, result);
		}
	}
}
