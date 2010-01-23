/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


using System;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.Operands
{
	/// <summary>
	/// An operand, which represents a string label in the program data.
	/// </summary>
	public sealed class StringLabelOperand : Operand
	{
		#region Data members

		/// <summary>
		/// Holds the name of the label.
		/// </summary>
		private string _name;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StringLabelOperand"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		public StringLabelOperand(SigType type, string name)
			: base(type)
		{
			this._name = name;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return this._name; }
		}

		#endregion // Properties

		#region Object Overrides

		/// <summary>
		/// Compares with the given operand for equality.
		/// </summary>
		/// <param name="other">The other operand to compare with.</param>
		/// <returns>The return value is true if the operands are equal; false if not.</returns>
		public override bool Equals(Operand other)
		{
			StringLabelOperand lop = other as StringLabelOperand;

			if (lop == null || lop.Type != Type)
				return false;

			if (_name == null && lop._name == null)
				return true;
			else
				return _name == lop._name;
		}

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return base.ToString().Replace("[", _name);
		}

		#endregion // Object Overrides
	}
}


