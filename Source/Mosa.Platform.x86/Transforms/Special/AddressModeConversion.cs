// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Xml.Linq;
using dnlib.DotNet.Emit;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Rewrite;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.Transforms.Special
{
	/// <summary>
	/// AddressModeConversion
	/// </summary>
	public sealed class AddressModeConversion : BaseTransform
	{
		public AddressModeConversion() : base(null, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ResultCount == 0)
				return false;

			if (context.OperandCount == 0)
				return false;

			if (!context.Instruction.IsPlatformInstruction)
				return false;

			if (context.Result == context.Operand1)
				return false;

			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == context.Operand1.Register)
				return false;

			return (context.Instruction as X86Instruction)?.ThreeTwoAddressConversion == true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var move = GetMoveFromType(result.Type);

			context.InsertBefore().SetInstruction(move, result, operand1);
			context.Operand1 = result;
		}

		private static BaseInstruction GetMoveFromType(MosaType type)
		{
			if (type.IsR4)
			{
				return X86.Movss;
			}
			else if (type.IsR8)
			{
				return X86.Movsd;
			}

			return X86.Mov32;
		}
	}
}
