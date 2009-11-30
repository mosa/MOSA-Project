using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class RuntimeMethodData
	{
		/// <summary>
		/// Maximum number of additional operands
		/// </summary>
		public const int MaxOperands = 255 - 3;

		#region Data members

		/// <summary>
		/// 
		/// </summary>
		public RuntimeMethod RuntimeMethod;

		/// <summary>
		/// Contains the label to apply to the data.
		/// </summary>
		public Operand[] AdditionalOperands = new Operand[MaxOperands];

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeMethodData"/> class.
		/// </summary>
		public RuntimeMethodData()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeMethodData"/> class.
		/// </summary>
		/// <param name="runtimeMethod">The runtime method.</param>
		public RuntimeMethodData(RuntimeMethod runtimeMethod)
		{
			RuntimeMethod = runtimeMethod;
		}

		#endregion // Construction

		#region Properties

		#endregion // Properties
	}
}
