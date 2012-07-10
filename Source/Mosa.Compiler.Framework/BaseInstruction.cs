/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the default number of operands for this instruction.
		/// </summary>
		protected byte operandDefaultCount;

		/// <summary>
		/// Holds the default number of operand results for this instruction.
		/// </summary>
		protected byte resultDefaultCount;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the default operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		public byte DefaultOperandCount { get { return operandDefaultCount; } }

		/// <summary>
		/// Gets the default result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		public byte DefaultResultCount { get { return resultDefaultCount; } }

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public virtual FlowControl FlowControl { get { return FlowControl.Next; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		public BaseInstruction()
		{
			operandDefaultCount = 0;
			resultDefaultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		public BaseInstruction(byte operandCount)
		{
			operandDefaultCount = operandCount;
			resultDefaultCount = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public BaseInstruction(byte operandCount, byte resultCount)
		{
			resultDefaultCount = resultCount;
			operandDefaultCount = operandCount;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public virtual void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			/* Default implementation is to do nothing */
		}

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string inst = GetType().ToString();

			int index = inst.LastIndexOf(".");

			if (index > 0)
				inst = inst.Substring(index + 1);

			index = inst.IndexOf("Instruction");

			if (index > 0)
				inst = inst.Substring(0, index);

			return inst;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public virtual string ToString(Context context)
		{
			StringBuilder s = new StringBuilder(ToString());

			if (context.Other is IR.ConditionCode)
			{
				s.Append(" [");
				s.Append(GetConditionString(context.ConditionCode));
				s.Append("]");
			}

			string mod = GetModifier(context);
			if (mod != null)
			{
				s.Append(" [");
				s.Append(mod);
				s.Append("]");
			}

			if (context.ResultCount != 0)
			{
				s.Append(" ");
				s.Append(context.Result);
			}

			if (context.ResultCount > 0 && context.OperandCount > 0)
			{
				s.Append(" <-");
			}

			for (int i = 0; (i < 3) && (i < context.OperandCount); i++)
			{
				s.Append(" ");
				s.Append(context.GetOperand(i));
				s.Append(",");
			}

			if (context.OperandCount > 3)
			{
				s.Append(" [more]");
			}
			else if (context.OperandCount > 0)
			{
				s.Length = s.Length - 1;
			}

			if (context.BranchTargets != null)
			{
				for (int i = 0; (i < 2) && (i < context.BranchTargets.Length); i++)
				{
					s.Append(String.Format(@" L_{0:X4},", context.BranchTargets[i]));
				}

				if (context.BranchTargets.Length > 2)
				{
					s.Append(" [more]");
				}
				else if (context.BranchTargets.Length > 0)
				{
					s.Length = s.Length - 1;
				}
			}

			if (context.InvokeTarget != null)
			{
				s.Append(" {");
				s.Append(context.InvokeTarget.ToString());
				s.Append("}");
			}

			if (context.RuntimeField != null)
			{
				s.Append(" {");
				s.Append(context.RuntimeField.ToString());
				s.Append("}");
			}

			return s.ToString();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IVisitor visitor, Context context)
		{
		}

		/// <summary>
		/// Gets the instruction modifier.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected virtual string GetModifier(Context context)
		{
			return null;
		}

		/// <summary>
		/// Gets the condition string.
		/// </summary>
		/// <param name="conditioncode">The conditioncode.</param>
		/// <returns></returns>
		protected string GetConditionString(IR.ConditionCode conditioncode)
		{
			switch (conditioncode)
			{
				case IR.ConditionCode.Equal: return @"equal";
				case IR.ConditionCode.GreaterOrEqual: return @"greater or equal";
				case IR.ConditionCode.GreaterThan: return @"greater";
				case IR.ConditionCode.LessOrEqual: return @"less or equal";
				case IR.ConditionCode.LessThan: return @"less";
				case IR.ConditionCode.NotEqual: return @"not equal";
				case IR.ConditionCode.UnsignedGreaterOrEqual: return @"greater or equal (U)";
				case IR.ConditionCode.UnsignedGreaterThan: return @"greater (U)";
				case IR.ConditionCode.UnsignedLessOrEqual: return @"less or equal (U)";
				case IR.ConditionCode.UnsignedLessThan: return @"less (U)";
				case IR.ConditionCode.NotSigned: return @"unsigned";
				case IR.ConditionCode.Signed: return @"signed";
				case IR.ConditionCode.Zero: return @"zero";
				case IR.ConditionCode.NoZero: return @"nozero";
				case IR.ConditionCode.NoParity: return @"noparity";
				case IR.ConditionCode.Carry: return @"carry";
				case IR.ConditionCode.NoCarry: return @"nocarry";

				default: throw new NotSupportedException();
			}
		}

		#endregion //  Methods
	}
}
