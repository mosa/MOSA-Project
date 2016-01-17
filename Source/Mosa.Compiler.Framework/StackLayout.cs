// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class StackLayout
	{
		#region Data members

		private BaseArchitecture architecture;

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
		/// Gets or sets the size of the stack parameter.
		/// </summary>
		/// <value>
		/// The size of the stack parameter.
		/// </value>
		public int StackParameterSize { get; set; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get; private set; }

		/// <summary>
		/// Gets the stack.
		/// </summary>
		public List<Operand> LocalStack { get; private set; }

		#endregion Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="StackLayout"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="parameters">The parameters.</param>
		public StackLayout(BaseArchitecture architecture, int parameters)
		{
			this.architecture = architecture;
			Parameters = new Operand[parameters];
			LocalStack = new List<Operand>();
		}

		/// <summary>
		/// Adds the stack local.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AddStackLocal(MosaType type)
		{
			var local = Operand.CreateStackLocal(type, architecture.StackFrameRegister, LocalStack.Count);
			LocalStack.Add(local);
			return local;
		}

		/// <summary>
		/// Sets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="type">The type.</param>
		/// <param name="displacement">The displacement.</param>
		/// <returns></returns>
		public Operand SetStackParameter(int index, MosaType type, int displacement, string name)
		{
			Parameters[index] = Operand.CreateParameter(type, architecture.StackFrameRegister, displacement, index, name);
			return Parameters[index];
		}
	}
}
