// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation
{
	public abstract class BaseTransformation
	{
		#region Properties

		public BaseInstruction Instruction { get; private set; }
		public List<BaseInstruction> Instructions { get; private set; }

		public string Name { get; }

		#endregion Properties

		#region Data Fields

		public List<OperandFilter> OperandFilters;

		#endregion Data Fields

		#region Constructors

		protected BaseTransformation()
		{
			Name = ExtractName();
			TransformationDirectory.Add(this);
		}

		public BaseTransformation(BaseInstruction instruction)
			: this()
		{
			Instruction = instruction;
		}

		public BaseTransformation(List<BaseInstruction> instructions)
			: this()
		{
			Instructions = instructions;
		}

		public BaseTransformation(BaseInstruction instruction, OperandFilter operand1 = null, OperandFilter operand2 = null, OperandFilter operand3 = null, OperandFilter operand4 = null)
			: this(instruction)
		{
			if (operand1 != null)
			{
				OperandFilters = new List<OperandFilter>();
			}

			if (operand1 != null)
				OperandFilters.Add(operand1);

			if (operand2 != null)
				OperandFilters.Add(operand2);

			if (operand3 != null)
				OperandFilters.Add(operand3);

			if (operand4 != null)
				OperandFilters.Add(operand4);
		}

		public BaseTransformation(List<BaseInstruction> instructions, OperandFilter operand1 = null, OperandFilter operand2 = null, OperandFilter operand3 = null, OperandFilter operand4 = null)
			: this(instructions)
		{
			if (operand1 != null)
			{
				OperandFilters = new List<OperandFilter>();
			}

			if (operand1 != null)
				OperandFilters.Add(operand1);

			if (operand2 != null)
				OperandFilters.Add(operand2);

			if (operand3 != null)
				OperandFilters.Add(operand3);

			if (operand4 != null)
				OperandFilters.Add(operand4);
		}

		#endregion Constructors

		#region Internals

		private string ExtractName()
		{
			string name = GetType().FullName;

			int offset1 = name.IndexOf('.');
			int offset2 = name.IndexOf('.', offset1);
			int offset3 = name.IndexOf('.', offset2);
			int offset4 = name.IndexOf('.', offset3);

			return name.Substring(offset4 + 1);
		}

		#endregion Internals

		public bool ValidateInstruction(Context context)
		{
			if (context.IsEmpty)
				return false;

			return context.Instruction == Instruction;
		}

		public virtual bool Match(Context context, TransformContext transformContext)
		{
			// Default - built in match

			if (OperandFilters != null)
			{
				// operand counts must match
				if (OperandFilters.Count != context.OperandCount)
					return false;

				if (OperandFilters.Count >= 1 && !OperandFilters[0].Compare(context.Operand1))
					return false;

				if (OperandFilters.Count >= 2 && !OperandFilters[1].Compare(context.Operand2))
					return false;

				if (OperandFilters.Count >= 3 && !OperandFilters[2].Compare(context.Operand3))
					return false;

				for (int i = 3; i < OperandFilters.Count; i++)
				{
					if (!OperandFilters[i].Compare(context.GetOperand(i)))
						return false;
				}

				return true;
			}

			return false;
		}

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

		protected static BaseInstruction GetMove(Operand operand)
		{
			if (operand.IsR4)
				return IRInstruction.MoveFloatR4;
			else if (operand.IsR8)
				return IRInstruction.MoveFloatR8;
			else if (operand.Is64BitInteger)
				return IRInstruction.MoveInt64;
			else
				return IRInstruction.MoveInt32;
		}

		protected static Operand GetZero(MosaType type, Operand operand)
		{
			if (operand.IsR4)
				return Operand.CreateConstant(type, 0.0f);
			else if (operand.IsR8)
				return Operand.CreateConstant(type, 0.0d);
			else if (operand.Is64BitInteger)
				return Operand.CreateConstant(type, 0);
			else
				return Operand.CreateConstant(type, 0);
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
