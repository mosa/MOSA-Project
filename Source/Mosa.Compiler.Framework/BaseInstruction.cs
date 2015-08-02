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
		#region Properties

		/// <summary>
		/// Gets the default operand count of the instruction
		/// </summary>
		/// <value>The operand count.</value>
		public byte DefaultOperandCount { get; protected set; }

		/// <summary>
		/// Gets the default result operand count of the instruction
		/// </summary>
		/// <value>The operand result count.</value>
		public byte DefaultResultCount { get; protected set; }

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

		/// <summary>
		/// Gets a value indicating whether to [ignore instruction's basic block].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore instruction basic block]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IgnoreInstructionBasicBlockTargets { get { return false; } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		public BaseInstruction(byte resultCount, byte operandCount)
		{
			DefaultResultCount = resultCount;
			DefaultOperandCount = operandCount;
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
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(InstructionNode node)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("L_{0:X4}", node.Label);

			if (node.Marked)
				sb.Append('*');
			else
				sb.Append(' ');

			sb.Append(ToString());

			var size = GetSizeString(node.Size);

			if (size != string.Empty)
				sb.Append("/" + size);

			if (node.ConditionCode != ConditionCode.Undefined)
			{
				sb.Append(" [");
				sb.Append(GetConditionString(node.ConditionCode));
				sb.Append("]");
			}

			if (node.MosaType != null)
			{
				sb.Append(" [[");
				sb.Append(node.MosaType.FullName);
				sb.Append("]]");
			}

			if (node.MosaField != null)
			{
				sb.Append(" [[");
				sb.Append(node.MosaField.FullName);
				sb.Append("]]");
			}

			string mod = GetModifier(node);
			if (mod != null)
			{
				sb.Append(" [");
				sb.Append(mod);
				sb.Append("]");
			}

			for (int i = 0; i < node.ResultCount; i++)
			{
				var op = node.GetResult(i);
				sb.Append(" ");
				sb.Append(op == null ? "[NULL]" : op.ToString());
				sb.Append(",");
			}

			if (node.ResultCount > 0)
			{
				sb.Length = sb.Length - 1;
			}

			if (node.ResultCount > 0 && node.OperandCount > 0)
			{
				sb.Append(" <=");
			}

			for (int i = 0; i < node.OperandCount; i++)
			{
				var op = node.GetOperand(i);
				sb.Append(" ");
				sb.Append(op == null ? "[NULL]" : op.ToString());
				sb.Append(",");
			}

			if (node.OperandCount > 0)
			{
				sb.Length = sb.Length - 1;
			}

			if (node.BranchTargets != null)
			{
				sb.Append(' ');

				for (int i = 0; (i < 2) && (i < node.BranchTargetsCount); i++)
				{
					if (i != 0)
					{
						sb.Append(", ");
					}

					sb.Append(node.BranchTargets[i].ToString());
				}

				if (node.BranchTargetsCount > 2)
				{
					sb.Append(", [more]");
				}
			}

			if (node.InvokeMethod != null)
			{
				sb.Append(" {");
				sb.Append(node.InvokeMethod.FullName);
				sb.Append("}");
			}

			if (node.MosaField != null)
			{
				sb.Append(" {");
				sb.Append(node.MosaField.FullName);
				sb.Append("}");
			}

			return sb.ToString();
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
		/// <param name="node">The node.</param>
		/// <returns></returns>
		protected virtual string GetModifier(InstructionNode node)
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
				case InstructionSize.Size128: return "128";
				case InstructionSize.Native: return "Native";
				default: return string.Empty;
			}
		}

		#endregion Methods
	}
}