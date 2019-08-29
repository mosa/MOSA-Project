// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

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

		#region Set Constant Result Helpers

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

		#endregion Set Constant Result Helpers

		protected static bool ValidateSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}

		#region SignExtend Helpers

		protected static uint SignExtend8x32(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFF00);
		}

		protected static uint SignExtend16x32(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFF0000);
		}

		protected static ulong SignExtend8x64(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFFFFFFFFFF00ul);
		}

		protected static ulong SignExtend16x64(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFFFFFFFFFF0000ul);
		}

		protected static ulong SignExtend32x64(uint value)
		{
			return ((value & 0x80000000) == 0) ? value : (value | 0xFFFFFFFF00000000ul);
		}

		#endregion SignExtend Helpers
	}
}
