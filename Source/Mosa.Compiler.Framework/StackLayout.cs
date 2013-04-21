/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

// NOTE: Eventually all temporary stack locals will be converted to virtual registers and
//       removed from this StackLayout class except for spill slots

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class StackLayout
	{
		#region Data members

		private IArchitecture architecture;

		private List<Operand> stackLocal = new List<Operand>();

		private Operand[] parameters;

		#endregion Data members

		#region Properties

		/// <summary>
		/// Gets or sets the size of the stack memory.
		/// </summary>
		/// <value>
		/// The size of the stack memory.
		/// </value>
		public int StackSize { get; set; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get { return parameters; } }

		/// <summary>
		/// Gets the stack.
		/// </summary>
		public IList<Operand> Stack { get { return stackLocal.AsReadOnly(); } }

		/// <summary>
		/// Gets the stack local count.
		/// </summary>
		public int StackLocalCount { get { return stackLocal.Count; } }

		#endregion Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="StackLayout"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="parameters">The parameters.</param>
		public StackLayout(IArchitecture architecture, int parameters)
		{
			this.architecture = architecture;
			this.parameters = new Operand[parameters];
		}

		/// <summary>
		/// Gets the stack local operand.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		public Operand GetStackLocalOperand(int slot)
		{
			return stackLocal[slot];
		}

		/// <summary>
		/// Adds the stack local.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AddStackLocal(SigType type)
		{
			return Operand.CreateStackLocal(type, stackLocal.Count);
		}

		/// <summary>
		/// Sets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="param">The param.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand SetStackParameter(int index, RuntimeParameter param, SigType type)
		{
			parameters[index] = Operand.CreateParameter(type, architecture.StackFrameRegister, param, index);
			return parameters[index];
		}

		/// <summary>
		/// Gets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetStackParameter(int index)
		{
			return parameters[index];
		}
	}
}