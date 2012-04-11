/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class StackLayout
	{

		#region Data members

		private IArchitecture architecture;

		private int stackMemorySize = 0;

		private List<StackOperand> stack = new List<StackOperand>();
		
		private ParameterOperand[] parameters;

		private StackSizeOperand stackSizeOperand;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the size of the stack.
		/// </summary>
		/// <value>
		/// The size of the stack.
		/// </value>
		public int StackMemorySize { get { return stackMemorySize; } set { stackMemorySize = value; } }

		/// <summary>
		/// Gets the stack size operand.
		/// </summary>
		public StackSizeOperand StackSizeOperand { get { return stackSizeOperand; } }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="StackLayout"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="parameters">The parameters.</param>
		public StackLayout(IArchitecture architecture, int parameters)
		{
			this.architecture = architecture;
			this.stackSizeOperand = new StackSizeOperand(this);
			this.parameters = new ParameterOperand[parameters];
		}

		/// <summary>
		/// Allocates the stack operand.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public StackOperand AllocateStackOperand(SigType type)
		{
			int stackSlot = stack.Count + 1;

			LocalVariableOperand local = new LocalVariableOperand(architecture.StackFrameRegister, String.Format("L_{0}", stackSlot), stackSlot, type);

			stack.Add(local);

			return local;
		}

		/// <summary>
		/// Gets the stack operand.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		public StackOperand GetStackOperand(int slot)
		{
			return stack[slot];
		}

		/// <summary>
		/// Sets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="param">The param.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public ParameterOperand SetStackParameter(int index, RuntimeParameter param, SigType type)
		{
			parameters[index] = new ParameterOperand(architecture.StackFrameRegister, param, type);
			return parameters[index];
		}

		/// <summary>
		/// Gets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public StackOperand GetStackParameter(int index)
		{
			return parameters[index];
		}

	}
}
