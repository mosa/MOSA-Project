/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public struct InstructionData
	{
		#region Data members

		/// <summary>
		/// Holds the instruction type of this instruction
		/// </summary>
		public IInstruction Instruction;

		/// <summary>
		/// Label of the instruction
		/// </summary>
		public int Label;

		/// <summary>
		/// Offset of the instruction from the start of the method.
		/// </summary>
		private int _offset;

		/// <summary>
		/// Holds the first operand of the instruction.
		/// </summary>
		public Operand Operand1;

		/// <summary>
		/// Holds the second operand of the instruction.
		/// </summary>
		public Operand Operand2;

		/// <summary>
		/// Holds the third operand of the instruction.
		/// </summary>
		public Operand Operand3;

		/// <summary>
		/// Holds the result operands of the instruction.
		/// </summary>
		public Operand Result;

		/// <summary>
		///  Holds the branch target information
		/// </summary>
		public IBranch Branch;


		/// <summary>
		/// Holds the "other" object
		/// </summary>
		public object Other;

		/// <summary>
		/// 
		/// </summary>
		private uint _packed;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets or sets the ignored attribute
		/// </summary>
		public bool Ignore
		{
			get { return (_packed & 0x01) == 0x01; }
			set { if (value) _packed = _packed | 0x01; else _packed = (uint)(_packed & ~0x1); }
		}

		/// <summary>
		/// Gets or sets the branch hint (true means branch likely)
		/// </summary>
		public bool BranchHint
		{
			get { return (_packed & 0x04) == 0x04; }
			set { if (value) _packed = _packed | 0x04; else _packed = (uint)(_packed & ~0x04); }
		}

		/// <summary>
		/// Gets or sets the number of operand results
		/// </summary>
		public byte ResultCount
		{
			get { return (byte)((_packed >> 16) & 0xF); }
			set { _packed = (uint)((_packed & 0xFF00FFFF) | ((uint)value << 16)); }
		}

		/// <summary>
		/// Gets or sets the number of operands
		/// </summary>
		public byte OperandCount
		{
			get { return (byte)((_packed >> 24) & 0xF); }
			set { _packed = (uint)((_packed & 0x00FFFFFF) | ((uint)value << 24)); }
		}

		/// <summary>
		/// Gets or sets the label of the instruction.
		/// </summary>
		/// <remarks>
		/// Offsets are used by branch instructions to define their target. During basic block
		/// building these offsets are used to insert labels at appropriate positions and the
		/// jumps or modified to target one of these labels. During code generation, the offset
		/// can be used to indicate native code offsets.
		/// </remarks>
		public int Offset
		{
			get { return _offset; }
			set
			{
				if (value < -1)
					throw new ArgumentOutOfRangeException(@"Offset can not be negative.", @"value");

				_offset = value;
			}
		}

		/// <summary>
		/// Gets or sets the invoke target.
		/// </summary>
		/// <value>The invoke target.</value>
		public RuntimeMethod InvokeTarget
		{
			get
			{
				if (!(Other is RuntimeMethodData)) return null;
				return (Other as RuntimeMethodData).RuntimeMethod;
			}
			set
			{
				if (!(Other is RuntimeMethodData)) Other = new RuntimeMethodData(value);
				else (Other as RuntimeMethodData).RuntimeMethod = value;
			}
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public RuntimeField RuntimeField
		{
			get { return Other as RuntimeField; }
			set { Other = value; }
		}

		/// <summary>
		/// Holds the token type.
		/// </summary>
		/// <value>The token.</value>
		public MetadataToken Token
		{
			get { return (MetadataToken)Other; }
			set { Other = value; }
		}

		/// <summary>
		/// Holds the token type.
		/// </summary>
		/// <value>The token.</value>
		public TokenTypes TokenType
		{
			get { return (TokenTypes)Other; }
			set { Other = value; }
		}

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public IR.ConditionCode ConditionCode
		{
			get { return (IR.ConditionCode)Other; }
			set { Other = value; }
		}

		/// <summary>
		/// Holds the literal data.
		/// </summary>
		/// <value>The token.</value>
		public IR.LiteralData LiteralData
		{
			get { return (IR.LiteralData)Other; }
			set { Other = value; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			this.Label = -1;
			this.Offset = 0;
			this.Instruction = null;
			this.Operand1 = null;
			this.Operand2 = null;
			this.Operand3 = null;
			this.Result = null;
			this._packed = 0;
			this.Branch = null;
			this.Other = null;
			this.BranchHint = false;
		}

		/// <summary>
		/// Clears the instance.
		/// </summary>
		public void ClearAbbreviated()
		{
			this.Label = -1;
			this.Offset = 0;
			this.Instruction = null;
			this.Ignore = true;
			this.OperandCount = 0;
			this.ResultCount = 0;
			this.Branch = null;
			this.Other = null;
		}

		/// <summary>
		/// Sets the addition operand.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetAdditionalOperand(int index, Operand operand)
		{
			if (Other == null) Other = new RuntimeMethodData();

			Debug.Assert(index < RuntimeMethodData.MaxOperands, @"No index");
			Debug.Assert(index >= 3, @"No index");

			(Other as RuntimeMethodData).AdditionalOperands[index - 3] = operand;
		}

		/// <summary>
		/// Gets the addition operand.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetAdditionalOperand(int index)
		{
			if (Other == null) return null;

			Debug.Assert(index < RuntimeMethodData.MaxOperands, @"No index");
			Debug.Assert(index >= 3, @"No index");

			return (Other as RuntimeMethodData).AdditionalOperands[index - 3];
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (Instruction == null)
				return "null";

			string str = Instruction.ToString();

			if (Label >= 0)
				str = Label.ToString() + ": " + str;

			if (Branch != null)
			{
				str = str + " (";
				foreach (int branch in Branch.Targets)
					str = str + branch.ToString() + ",";

				str = str.Trim(',') + ")";
			}

			return str;
		}

		#endregion
	}
}
