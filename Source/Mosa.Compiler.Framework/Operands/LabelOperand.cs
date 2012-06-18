/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Operands
{
	/// <summary>
	/// An operand, which represents a label in the program data.
	/// </summary>
	public sealed class LabelOperand : MemoryOperand
	{
		#region Data members

		/// <summary>
		/// Holds the name
		/// </summary>
		private string name;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelOperand"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		public LabelOperand(SigType type, string name) :
			base(null, type, IntPtr.Zero)
		{
			this.name = name;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the label of the operand.
		/// </summary>
		/// <value>The label.</value>
		public string Name
		{
			get { return name; }
		}

		#endregion // Properties

		#region Object Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return name + " " + base.ToString();
		}

		#endregion // Object Overrides
	}
}


