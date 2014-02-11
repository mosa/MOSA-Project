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

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PtrSigType"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="customMods">The custom mods.</param>
		public PtrSigType(SigType type, CustomMod[] customMods)
			: base(CilElementType.Ptr)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			this.customMods = customMods;
			this.elementType = type;
		}

		#endregion Construction

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

		#endregion Properties

		/// <summary>
		/// Expresses the pointer type reference signature component in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			if (this.ElementType.IsVoid)
			{
				// NOTE: Void should only ever be written for a symbol when used as a pointer in a signature
				sb.Append("void*");
			}
			else
			{
				sb.Append(ElementType.ToSymbolPart());
				sb.Append('*');
			}

			return sb.ToString();
		}
	}
}