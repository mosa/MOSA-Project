// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Auto.Shrink
{
	/// <summary>
	/// And32ByFF
	/// </summary>
	public sealed class And32ByFF : BaseTransformation
	{
		public And32ByFF() : base(X86.And32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0xFF)
				return false;

			if (IsCPURegister(context.Operand1, CPURegister.ESI))
				return false;

			if (IsCPURegister(context.Operand1, CPURegister.EDI))
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(X86.Movzx8To32, result, t1);
		}
	}

	/// <summary>
	/// And32ByFF_v1
	/// </summary>
	public sealed class And32ByFF_v1 : BaseTransformation
	{
		public And32ByFF_v1() : base(X86.And32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0xFF)
				return false;

			if (IsCPURegister(context.Operand2, CPURegister.ESI))
				return false;

			if (IsCPURegister(context.Operand2, CPURegister.EDI))
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand2;

			context.SetInstruction(X86.Movzx8To32, result, t1);
		}
	}
}
