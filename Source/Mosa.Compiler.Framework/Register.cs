// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Represents a machine specific abstract register.
	/// </summary>
	public abstract class Register
	{
		#region Data members

		/// <summary>
		/// Holds the register index.
		/// </summary>
		private int index;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Register"/>.
		/// </summary>
		/// <param name="index">The numeric index of the register.</param>
		protected Register(int index)
		{
			this.index = index;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Retrieves the numeric index of this register in its architecture.
		/// </summary>
		public int Index
		{
			get { return index; }
		}

		/// <summary>
		/// Determines if this is a floating point register.
		/// </summary>
		public abstract bool IsFloatingPoint { get; }

		/// <summary>
		/// Determines if this is a integer register.
		/// </summary>
		public abstract bool IsInteger { get; }

		/// <summary>
		/// Holds the machine specific index or code of the register.
		/// </summary>
		public abstract int RegisterCode { get; }

		/// <summary>
		/// Returns the width of the register in bits.
		/// </summary>
		public abstract int Width { get; }

		/// <summary>
		/// Gets a value indicating whether this register is special register that the
		/// registor allocator should not consider.
		/// </summary>
		public bool IsSpecial { get { return !(IsInteger || IsFloatingPoint); } }

		#endregion Properties
	}
}
