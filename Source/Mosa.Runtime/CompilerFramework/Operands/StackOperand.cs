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
	/// Represents an operand, that is located on the relative to the current stack frame.
	/// </summary>
	public abstract class StackOperand : MemoryOperand
	{
		#region Data members

		/// <summary>
		/// Holds the SSA version of the stack operand.
		/// </summary>
		private int _ssaVersion;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="StackOperand"/>.
		/// </summary>
		/// <param name="type">Holds the type of data held in this operand.</param>
		/// <param name="register">Holds the stack frame register.</param>
		/// <param name="offset">The offset of the variable on stack. A positive value reflects the current function stack, a negative offset indicates a parameter.</param>
		protected StackOperand(SigType type, Register register, int offset) :
			base(type, register, new IntPtr(offset * 4))
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the name of the stack operand.
		/// </summary>
		/// <value>The name of the stack operand.</value>
		public abstract string Name { get; }

		/// <summary>
		/// Gets or sets the SSA version of the operand.
		/// </summary>
		/// <value>The version of the stack operand.</value>
		public int Version
		{
			get { return _ssaVersion; }
			set { _ssaVersion = value; }
		}

		#endregion // Properties

		#region Operand Overrides

		/// <summary>
		/// Compares with the given operand for equality.
		/// </summary>
		/// <param name="other">The other operand to compare with.</param>
		/// <returns>The return value is true if the operands are equal; false if not.</returns>
		public override bool Equals(Operand other)
		{
			StackOperand sop = other as StackOperand;
			return (null != sop && sop.Type == this.Type && sop.Offset == this.Offset && sop.Base == this.Base && sop.Version == this.Version);
		}

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public sealed override string ToString()
		{
			string tmp = base.ToString();
			//return String.Format(@"{0} {1}", this.Name, tmp.Insert(tmp.Length - 1, String.Format(", SSA Version: {0}", _ssaVersion)));
			if (_ssaVersion == 0)
				return String.Format(@"{0} {1}", this.Name, tmp);
			else
				return String.Format(@"{0} {1}", this.Name, tmp.Insert(tmp.Length - 1, String.Format(" #{0}", _ssaVersion)));
		}

		#endregion // Operand Overrides

	}
}


