// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x64.Transform.Auto.Shrink
{
	/// <summary>
	/// And64ByFF
	/// </summary>
	public sealed class And64ByFF : BaseTransformation
	{
		public And64ByFF() : base(X64.And64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand2.IsResolvedConstant)
				return false;

			if (context.Operand2.ConstantUnsigned64 != 0xFF)
				return false;

			if (IsCPURegister(context.Operand1, CPURegister.RSI))
				return false;

			if (IsCPURegister(context.Operand1, CPURegister.RDI))
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;

			context.SetInstruction(X64.Movzx8To64, result, t1);
		}
	}

	/// <summary>
	/// And64ByFF_v1
	/// </summary>
	public sealed class And64ByFF_v1 : BaseTransformation
	{
		public And64ByFF_v1() : base(X64.And64, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsResolvedConstant)
				return false;

			if (context.Operand1.ConstantUnsigned64 != 0xFF)
				return false;

			if (IsCPURegister(context.Operand2, CPURegister.RSI))
				return false;

			if (IsCPURegister(context.Operand2, CPURegister.RDI))
				return false;

			if (AreStatusFlagUsed(context))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand2;

			context.SetInstruction(X64.Movzx8To64, result, t1);
		}
	}
}
