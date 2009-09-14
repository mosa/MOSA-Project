/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

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
		/// Holds the block index of this instruction.
		/// </summary>
		public int Block;

		/// <summary>
		/// Determines if this instruction is ignored.
		/// </summary>
		public bool Ignore;

		/// <summary>
		/// IL offset of the instruction From the start of the method.
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
		/// Holds the result operands of the instruction.
		/// </summary>
		public Operand Result2;

		/// <summary>
		/// Holds the prefix instruction of the instruction
		/// </summary>
		public IInstruction Prefix;

		/// <summary>
		///  Holds the branch target information
		/// </summary>
		public IBranch Branch;

		/// <summary>
		/// Holds the function being called.
		/// </summary>
		public RuntimeMethod InvokeTarget;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets or sets the offset of the instruction From the start of the method.
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
				if (value < 0)
					throw new ArgumentOutOfRangeException(@"Offset can not be negative.", @"value");

				_offset = value;
			}
		}

		#endregion // Properties
	}
}
