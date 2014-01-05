/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// The signature type of a zero-based, single-dimensional array type.
	/// </summary>
	public sealed class SZArraySigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SZArraySigType"/> class.
		/// </summary>
		/// <param name="customMods">The custom mods.</param>
		/// <param name="type">The type.</param>
		public SZArraySigType(CustomMod[] customMods, SigType type) :
			base(CilElementType.SZArray)
		{
			CustomMods = customMods;
			ElementType = type;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the custom modifiers of the array type.
		/// </summary>
		/// <value>The custom modifiers of the array type.</value>
		public CustomMod[] CustomMods { get; private set; }

		/// <summary>
		/// Gets the type of the array elements.
		/// </summary>
		/// <value>The type of the array elements.</value>
		public SigType ElementType { get; private set; }

		#endregion Properties

		/// <summary>
		/// Expresses the array type reference in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(ElementType.ToSymbolPart());
			sb.Append("[]");
			return sb.ToString();
		}
	}
}