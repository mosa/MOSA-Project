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

		/// <summary>
		/// Gets a value indicating whether to [ignore during code generation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore during code generation]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IgnoreDuringCodeGeneration { get { return false; } }

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
			var s = new StringBuilder(ToString());

			var size = GetSizeString(context.Size);

			if (size != string.Empty)
				s.Append("/" + size);

			if (context.ConditionCode != ConditionCode.Undefined)
			{
				s.Append(" [");
				s.Append(GetConditionString(context.ConditionCode));
				s.Append("]");
			}

			if (context.MosaType != null)
			{
				s.Append(" [[");
				s.Append(context.MosaType.FullName);
				s.Append("]]");
			}

			if (context.MosaField != null)
			{
				s.Append(" [[");
				s.Append(context.MosaField.FullName);
				s.Append("]]");
			}

			string mod = GetModifier(context);
			if (mod != null)
			{
				s.Append(" [");
				s.Append(mod);
				s.Append("]");
			}

			for (int i = 0; i < context.ResultCount; i++)
			{
				var op = context.GetResult(i);
				s.Append(" ");
				s.Append(op == null ? "[NULL]" : op.ToString());
				s.Append(",");
			}

			if (context.ResultCount > 0)
			{
				s.Length = s.Length - 1;
			}

			if (context.ResultCount > 0 && context.OperandCount > 0)
			{
				s.Append(" <=");
			}

			for (int i = 0; i < context.OperandCount; i++)
			{
				var op = context.GetOperand(i);
				s.Append(" ");
				s.Append(op == null ? "[NULL]" : op.ToString());
				s.Append(",");
			}

			if (context.OperandCount > 0)
			{
				s.Length = s.Length - 1;
			}

			if (context.Targets != null)
			{
				for (int i = 0; (i < 2) && (i < context.Targets.Count); i++)
				{
					s.Append(' ');
					s.Append(context.Targets[i].ToString());
				}

				if (context.Targets.Count > 2)
				{
					s.Append(" [more]");
				}
				else if (context.Targets.Count > 0)
				{
					s.Length = s.Length - 1;
				}
			}

			if (context.MosaMethod != null)
			{
				s.Append(" {");
				s.Append(context.MosaMethod.FullName);
				s.Append("}");
			}

			if (context.MosaField != null)
			{
				s.Append(" {");
				s.Append(context.MosaField.FullName);
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
				case ConditionCode.Always: return @"always";

				default: throw new NotSupportedException();
			}
		}

		public static string GetSizeString(InstructionSize size)
		{
			switch (size)
			{
				case InstructionSize.Size32: return "32";
				case InstructionSize.Size8: return "8";
				case InstructionSize.Size16: return "16";
				case InstructionSize.Size64: return "64";
				case InstructionSize.Native: return "Native";
				default: return string.Empty;
			}
		}

		#endregion Methods
	}
}
