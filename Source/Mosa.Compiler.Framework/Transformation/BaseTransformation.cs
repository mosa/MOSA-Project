// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Transformation
{
	public abstract class BaseTransformation
	{
		public virtual BaseInstruction Instruction { get; private set; }

		public string Name { get; }

		protected BaseTransformation()
		{
			Name = ExtractName();
		}

		private string ExtractName()
		{
			string name = GetType().FullName;

			int offset1 = name.IndexOf('.');
			int offset2 = name.IndexOf('.', offset1);
			int offset3 = name.IndexOf('.', offset2);
			int offset4 = name.IndexOf('.', offset3);

			return name.Substring(offset4 + 1);
		}

		public bool ValidateInstruction(Context context)
		{
			if (context.IsEmpty)
				return false;

			return context.Instruction == Instruction;
		}

		public abstract bool Match(Context context, TransformContext transformContext);

		public abstract void Transform(Context context, TransformContext transformContext);

		//public bool IsFirstOperandResolvedConstant(Context context)
		//{
		//	return context.Operand1.IsResolvedConstant;
		//}

		//public bool AreFirstTwoOperandsResolvedConstants(Context context)
		//{
		//	return context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant;
		//}

		#region Helpers

		protected static void SetConstantResult(Context context, uint constant)
		{
			var result = context.Result;

			context.SetInstruction(IRInstruction.MoveInt32, result, ConstantOperand.Create(result.Type, constant));
		}

		protected static void SetConstantResult(Context context, ulong constant)
		{
			var result = context.Result;

			context.SetInstruction(IRInstruction.MoveInt64, result, ConstantOperand.Create(result.Type, constant));
		}

		protected static void SetConstantResult(Context context, float constant)
		{
			var result = context.Result;

			context.SetInstruction(IRInstruction.MoveFloatR4, result, ConstantOperand.Create(result.Type, constant));
		}

		protected static void SetConstantResult(Context context, double constant)
		{
			var result = context.Result;

			context.SetInstruction(IRInstruction.MoveFloatR8, result, ConstantOperand.Create(result.Type, constant));
		}

		#endregion Helpers
	}
}
