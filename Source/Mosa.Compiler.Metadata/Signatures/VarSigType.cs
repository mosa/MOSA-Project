/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// The signature type of a generic type parameter.
	/// </summary>
	public sealed class VarSigType : SigType
	{
		#region Properties

		/// <summary>
		/// Gets the generic type parameter index.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="VarSigType"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public VarSigType(int index) :
			base(CilElementType.Var)
		{
			Index = index;
		}

		#endregion Construction

		#region SigType Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + "#" + Index.ToString();
		}

		#endregion SigType Overrides
	}
}