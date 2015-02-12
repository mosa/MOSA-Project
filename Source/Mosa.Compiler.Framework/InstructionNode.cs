/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public sealed class InstructionNode
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
		/// The order slot number (initalized by some stage)
		/// </summary>
		public int SlotNumber;

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
		/// Holds the result first operand of the instruction.
		/// </summary>
		public Operand Result;

		/// <summary>
		/// Holds the second result operand of the instruction.
		/// </summary>
		public Operand Result2;

		/// <summary>
		/// The condition code
		/// </summary>
		public ConditionCode ConditionCode;

		/// <summary>
		///  Holds the branch targets
		/// </summary>
		public int[] BranchTargets;

		/// <summary>
		/// Holds branch targets
		/// </summary>
		public List<BasicBlock> Targets;

		/// <summary>
		/// The instruction size
		/// </summary>
		public InstructionSize Size;

		/// <summary>
		/// Holds a packed value (to save space)
		/// </summary>
		private uint packed;

		/// <summary>
		/// The additional properties of an instruction node
		/// </summary>
		private InstructionNodeExtension extension;

		#endregion Data members

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
		/// Gets or sets a value indicating whether this <see cref="InstructionNode"/> is marked.
		/// </summary>
		public bool Marked
		{
			get { return (packed & 0x08) == 0x08; }
			set { if (value) packed = packed | 0x08; else packed = (uint)(packed & ~0x08); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the insturction updates the status flag.
		/// </summary>
		/// <value>
		///   <c>true</c> if [set status flag]; otherwise, <c>false</c>.
		/// </value>
		public bool UpdateStatus
		{
			get { return (packed & 0x16) == 0x16; }
			set { if (value) packed = packed | 0x16; else packed = (uint)(packed & ~0x16); }
		}

		/// <summary>
		/// Gets or sets the number of operand results
		/// </summary>
		public byte ResultCount
		{
			get { return (byte)((packed >> 8) & 0xF); }
			set { packed = (uint)((packed & 0xFFFFF0FF) | ((uint)value << 8)); }
		}

		/// <summary>
		/// Gets or sets the number of operands
		/// </summary>
		public byte OperandCount
		{
			get { return (byte)((packed >> 20) & 0xFFF); }
			set { packed = (uint)((packed & 0x000FFFFF) | ((uint)value << 20)); }
		}

		private void CheckExtension()
		{
			if (extension == null)
			{
				extension = new InstructionNodeExtension();
			}
		}

		/// <summary>
		/// Gets or sets the invoke method.
		/// </summary>
		/// <value>
		/// The invoke method.
		/// </value>
		public MosaMethod InvokeMethod
		{
			get { return extension == null ? null : extension.InvokeMethod; }
			set { CheckExtension(); extension.InvokeMethod = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaField MosaField
		{
			get { return extension == null ? null : extension.MosaField; }
			set { CheckExtension(); extension.MosaField = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaType MosaType
		{
			get { return extension == null ? null : extension.MosaType; }
			set { CheckExtension(); extension.MosaType = value; }
		}

		/// <summary>
		///  Holds the cil branch targets
		/// </summary>
		public List<int> CILTargets
		{
			get { return extension == null ? null : extension.CILTargets; }
			set { CheckExtension(); extension.CILTargets = value; }
		}

		/// <summary>
		/// Gets or sets the phi blocks.
		/// </summary>
		/// <value>
		/// The phi blocks.
		/// </value>
		public List<BasicBlock> PhiBlocks
		{
			get { return extension == null ? null : extension.PhiBlocks; }
			set { CheckExtension(); extension.PhiBlocks = value; }
		}

		#endregion Properties

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
			this.Result2 = null;
			this.packed = 0;
			this.extension = null;
			this.Targets = null;
			this.BranchHint = false;
			this.ConditionCode = ConditionCode.Undefined;
		}

		/// <summary>
		/// Sets the additional operand.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetAdditionalOperand(int index, Operand operand)
		{
			CheckExtension();
			if (extension.AdditionalOperands == null) extension.AdditionalOperands = new Operand[253];

			Debug.Assert(index < 255, @"No Index");
			Debug.Assert(index >= 3, @"No Index");

			extension.AdditionalOperands[index - 3] = operand;
		}

		/// <summary>
		/// Gets the additional operand.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetAdditionalOperand(int index)
		{
			if (extension == null || extension.AdditionalOperands == null)
				return null;

			Debug.Assert(index < 255, @"No Index");
			Debug.Assert(index >= 3, @"No Index");

			return extension.AdditionalOperands[index - 3];
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

			if (Targets != null)
			{
				str = str + " (";
				foreach (var branch in Targets)
					str = str + branch.ToString() + ",";

				str = str.Trim(',') + ")";
			}

			return str;
		}

		#endregion Methods
	}
}
