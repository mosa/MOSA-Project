/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// Represents a generic parameter type of a generic member definition.
	/// </summary>
	public sealed class MVarSigType : SigType
	{
		#region Data members

		/// <summary>
		/// Holds the index of the generic parameter in the generic parameter list.
		/// </summary>
		private int index;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MVarSigType"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public MVarSigType(int index) :
			base(CilElementType.MVar)
		{
			this.index = index;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the index of the generic parameter in the generic parameter list of the generic member.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get { return index; } }

		#endregion // Properties

		#region SigType Overrides

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(SigType other)
		{
			MVarSigType mvst = other as MVarSigType;
			if (mvst == null)
				return false;

			return (base.Equals(other) == true && index == mvst.index);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + "#" + index.ToString();
		}

		#endregion // SigType Overrides
	}
}
