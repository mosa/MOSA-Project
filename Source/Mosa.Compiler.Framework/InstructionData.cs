/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Diagnostics;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
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
		public BaseInstruction Instruction;

		/// <summary>
		/// Label of the instruction
		/// </summary>
		public int Label;

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
		///  Holds the branch targets
		/// </summary>
		public int[] BranchTargets;

		/// <summary>
		/// Holds the "other" object
		/// </summary>
		public object Other;

		/// <summary>
		/// 
		/// </summary>
		private uint packed;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this instance has a prefix.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has a prefix; otherwise, <c>false</c>.
		/// </value>
		public bool HasPrefix
		{
			get { return (packed & 0x02) == 0x02; }
			set { if (value) packed = packed | 0x02; else packed = (uint)(packed & ~0x2); }
		}

		/// <summary>
		/// Gets or sets the branch hint (true means branch likely)
		/// </summary>
		public bool BranchHint
		{
			get { return (packed & 0x04) == 0x04; }
			set { if (value) packed = packed | 0x04; else packed = (uint)(packed & ~0x04); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="InstructionData"/> is marked.
		/// </summary>
		public bool Marked
		{
			get { return (packed & 0x08) == 0x08; }
			set { if (value) packed = packed | 0x08; else packed = (uint)(packed & ~0x08); }
		}

		/// <summary>
		/// Gets or sets the number of operand results
		/// </summary>
		public byte ResultCount
		{
			get { return (byte)((packed >> 16) & 0xF); }
			set { packed = (uint)((packed & 0xFF00FFFF) | ((uint)value << 16)); }
		}

		/// <summary>
		/// Gets or sets the number of operands
		/// </summary>
		public byte OperandCount
		{
			get { return (byte)((packed >> 24) & 0xF); }
			set { packed = (uint)((packed & 0x00FFFFFF) | ((uint)value << 24)); }
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
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public RuntimeType RuntimeType
		{
			get { return Other as RuntimeType; }
			set { Other = value; }
		}

		/// <summary>
		/// Gets or sets the sig type.
		/// </summary>
		/// <value>The runtime field.</value>
		public SigType SigType
		{
			get { return Other as SigType; }
			set { Other = value; }
		}

		/// <summary>
		/// Gets or sets the token type.
		/// </summary>
		/// <value>The token type.</value>
		public HeapIndexToken TokenType
		{
			get { return (HeapIndexToken)Other; }
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

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			this.Label = -1;
			this.Instruction = null;
			this.Operand1 = null;
			this.Operand2 = null;
			this.Operand3 = null;
			this.Result = null;
			this.packed = 0;
			this.BranchTargets = null;
			this.Other = null;
			this.BranchHint = false;
		}

		/// <summary>
		/// Clears the instance.
		/// </summary>
		public void ClearAbbreviated()
		{
			this.Label = -1;
			this.Instruction = null;
			this.OperandCount = 0;
			this.ResultCount = 0;
			this.BranchTargets = null;
			this.Other = null;
		}

		/// <summary>
		/// Sets the additional operand.
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
		/// Gets the additional operand.
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
		/// Allocates the branch targets.
		/// </summary>
		/// <param name="targets">The targets.</param>
		public void AllocateBranchTargets(uint targets)
		{
			BranchTargets = new int[targets];
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

			if (BranchTargets != null)
			{
				str = str + " (";
				foreach (int branch in BranchTargets)
					str = str + branch.ToString() + ",";

				str = str.Trim(',') + ")";
			}

			return str;
		}

		#endregion
	}
}
