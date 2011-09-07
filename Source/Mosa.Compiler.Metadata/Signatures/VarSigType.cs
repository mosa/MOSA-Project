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
	/// The signature type of a generic type parameter.
	/// </summary>
	public sealed class VarSigType : SigType
	{
		#region Data members

		/// <summary>
		/// The generic type parameter index.
		/// </summary>
		private int index;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="VarSigType"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public VarSigType(int index) :
			base(CilElementType.Var)
		{
			this.index = index;
		}

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the generic type parameter index.
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
			VarSigType vst = other as VarSigType;
			if (null == vst)
				return false;

			return (base.Equals(other) && index == vst.index);
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
