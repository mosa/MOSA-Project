/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// Pointer signature type.
	/// </summary>
	public sealed class PtrSigType : SigType
	{
		#region Data members

		/// <summary>
		/// Holds the modifiers of the pointer signature type.
		/// </summary>
		private CustomMod[] customMods;

		/// <summary>
		/// Specifies the type pointed to.
		/// </summary>
		private SigType elementType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PtrSigType"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public PtrSigType(SigType type)
			: base(CilElementType.Ptr)
		{
			if (null == type)
				throw new ArgumentNullException(@"type");

			this.customMods = null;
			this.elementType = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PtrSigType"/> class.
		/// </summary>
		/// <param name="customMods">The custom mods.</param>
		/// <param name="type">The type.</param>
		public PtrSigType(CustomMod[] customMods, SigType type)
			: base(CilElementType.Ptr)
		{
			if (null == type)
				throw new ArgumentNullException(@"type");

			this.customMods = customMods;
			this.elementType = type;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the custom modifiers of the pointer type.
		/// </summary>
		/// <value>The custom modifiers of the pointer type.</value>
		public CustomMod[] CustomMods
		{
			get { return this.customMods; }
		}

		/// <summary>
		/// Gets the type pointed to.
		/// </summary>
		/// <value>The type pointed to.</value>
		public SigType ElementType
		{
			get { return this.elementType; }
		}

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
			PtrSigType pother = other as PtrSigType;
			if (null == pother)
				return false;

			return (base.Equals(other) == true && this.elementType.Matches(pother.elementType) == true && CustomMod.Equals(this.customMods, pother.customMods));
		}

		/// <summary>
		/// Matches the specified other.
		/// </summary>
		/// <param name="other">The other signature type.</param>
		/// <returns>True, if the signature type matches.</returns>
		public override bool Matches(SigType other)
		{
			PtrSigType ptrOther = other as PtrSigType;
			// FIXME: Do we need to consider custom mods here?
			return (ptrOther != null && ptrOther.elementType.Matches(this.elementType) == true);
		}

		#endregion // SigType Overrides

		/// <summary>
		/// Expresses the pointer type reference signature component in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			if (this.ElementType.Type == CilElementType.Void)
			{
				// NOTE: Void should only ever be written for a symbol when used as a pointer in a signature
				sb.Append("void*");
			}
			else
			{
				sb.Append(this.ElementType.ToSymbolPart());
				sb.Append('*');
			}

			return sb.ToString();
		}
	}
}
