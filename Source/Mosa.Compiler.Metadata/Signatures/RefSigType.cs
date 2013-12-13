/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Reference signature type.
	/// </summary>
	public sealed class RefSigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RefSigType"/> class.
		/// </summary>
		/// <param name="type">The referenced type.</param>
		public RefSigType(SigType type) :
			base(CilElementType.ByRef)
		{
			if (null == type)
				throw new ArgumentNullException(@"type");

			ElementType = type;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the type referenced by this signature type.
		/// </summary>
		/// <value>The referenced type.</value>
		public SigType ElementType { get; private set; }

		#endregion Properties

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
			RefSigType rst = other as RefSigType;
			if (null == rst)
				return false;

			return (base.Equals(other) && ElementType.Matches(rst.ElementType) == true);
		}

		/// <summary>
		/// Matches the specified other.
		/// </summary>
		/// <param name="other">The other signature type.</param>
		/// <returns>True, if the signature type matches.</returns>
		public override bool Matches(SigType other)
		{
			RefSigType refOther = other as RefSigType;

			// FIXME: Do we need to consider custom mods here?
			return (refOther != null && refOther.ElementType.Matches(ElementType));
		}

		#endregion SigType Overrides

		/// <summary>
		/// Expresses the byref parameter signature component in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("ref ");
			sb.Append(ElementType.ToSymbolPart());
			return sb.ToString();
		}
	}
}