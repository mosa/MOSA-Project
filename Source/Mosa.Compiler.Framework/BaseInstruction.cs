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

		#endregion Data members

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

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		public BaseInstruction(byte resultCount, byte operandCount)
		{
			resultDefaultCount = resultCount;
			operandDefaultCount = operandCount;
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public virtual void Resolve(Context ctx, BaseMethodCompiler compiler)
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

			if (context.Other is ConditionCode)
			{
				s.Append(" [");
				s.Append(GetConditionString(context.ConditionCode));
				s.Append("]");
			}

			if (context.RuntimeType != null)
			{
				s.Append(" [");
				s.Append(context.RuntimeType.FullName);
				s.Append("]");
			}

			if (context.RuntimeField != null)
			{
				s.Append(" [");
				s.Append(context.RuntimeField.FullName);
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

				if (context.ResultCount == 2)
				{
					s.Append(" : ");
					s.Append(context.Result2);
				}
			}
			if (context.ResultCount > 0 && context.OperandCount > 0)
			{
				s.Append(" <=");
			}

			for (int i = 0; i < context.OperandCount; i++)
			{
				s.Append(" ");
				s.Append(context.GetOperand(i));
				s.Append(",");
			}

			//if (context.OperandCount > 4)
			//{
			//	s.Append(" [more]");
			//}
			//else
			if (context.OperandCount > 0)
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

			//if (context.InvokeMethod != null)
			//{
			//	s.Append(" {");
			//	s.Append(context.InvokeMethod.FullName);
			//	s.Append("}");
			//}

			if (context.RuntimeField != null)
			{
				s.Append(" {");
				s.Append(context.RuntimeField.FullName);
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
		protected string GetConditionString(ConditionCode conditioncode)
		{
			switch (conditioncode)
			{
				case ConditionCode.Equal: return @"equal";
				case ConditionCode.GreaterOrEqual: return @"greater or equal";
				case ConditionCode.GreaterThan: return @"greater";
				case ConditionCode.LessOrEqual: return @"less or equal";
				case ConditionCode.LessThan: return @"less";
				case ConditionCode.NotEqual: return @"not equal";
				case ConditionCode.UnsignedGreaterOrEqual: return @"greater or equal (U)";
				case ConditionCode.UnsignedGreaterThan: return @"greater (U)";
				case ConditionCode.UnsignedLessOrEqual: return @"less or equal (U)";
				case ConditionCode.UnsignedLessThan: return @"less (U)";
				case ConditionCode.NotSigned: return @"not signed";
				case ConditionCode.Signed: return @"signed";
				case ConditionCode.Zero: return @"zero";
				case ConditionCode.NotZero: return @"not zero";
				case ConditionCode.Parity: return @"parity";
				case ConditionCode.NoParity: return @"no parity";
				case ConditionCode.Carry: return @"carry";
				case ConditionCode.NoCarry: return @"no carry";

				default: throw new NotSupportedException();
			}
		}

		#endregion Methods
	}
}