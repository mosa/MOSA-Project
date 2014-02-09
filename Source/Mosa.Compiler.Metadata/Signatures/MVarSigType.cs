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
	/// Represents a generic parameter type of a generic member definition.
	/// </summary>
	public sealed class MVarSigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MVarSigType"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public MVarSigType(int index) :
			base(CilElementType.MVar)
		{
			Index = index;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the index of the generic parameter in the generic parameter list of the generic member.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get; private set; }

		#endregion Properties

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