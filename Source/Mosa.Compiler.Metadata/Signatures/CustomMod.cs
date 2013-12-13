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

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Specifies various custom modifier types.
	/// </summary>
	public enum CustomModType
	{
		/// <summary>
		/// No modification of the type.
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates support for the type is optional.
		/// </summary>
		Optional = 1,

		/// <summary>
		/// Indicates support for the type is mandatory.
		/// </summary>
		Required = 2
	}

	/// <summary>
	/// Specifies a modifier of a signature type.
	/// </summary>
	public struct CustomMod : IEquatable<CustomMod>
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomMod"/> struct.
		/// </summary>
		/// <param name="type">The modifier type.</param>
		/// <param name="token">The modifier token type.</param>
		public CustomMod(CustomModType type, Token token)
			: this()
		{
			Type = type;
			Token = token;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the custom modifier type.
		/// </summary>
		/// <value>The modifier type.</value>
		public CustomModType Type { get; private set; }

		/// <summary>
		/// Gets the custom modifiers token type.
		/// </summary>
		/// <value>The token type.</value>
		public Token Token { get; private set; }

		#endregion Properties

		#region Static methods

		/// <summary>
		/// Parses the custom mods.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static CustomMod[] ParseCustomMods(SignatureReader reader)
		{
			List<CustomMod> mods = new List<CustomMod>();
			while (reader.Index != reader.Length)
			{
				CilElementType type = (CilElementType)reader.PeekByte();
				if (type != CilElementType.Optional && type != CilElementType.Required)
					break;

				reader.SkipByte();

				Token modType = reader.ReadEncodedTypeDefOrRef();
				mods.Add(new CustomMod((CustomModType)(type - CilElementType.Required + 1), modType));
			}

			return mods.ToArray();
		}

		/// <summary>
		/// Compares two modifier sets for equality.
		/// </summary>
		/// <param name="first">The first set of modifiers.</param>
		/// <param name="second">The second set of modifiers.</param>
		/// <returns>The return value is true, if the sets are equal; otherwise false.</returns>
		public static bool Equals(CustomMod[] first, CustomMod[] second)
		{
			if (first == second)
				return true;
			if (first.Length != second.Length)
				return false;

			bool result = true;
			for (int idx = 0; result && idx < first.Length; idx++)
			{
				result = first[idx].Equals(second[idx]);
			}

			return result;
		}

		#endregion Static methods

		#region IEquatable<CustomMod> Members

		bool IEquatable<CustomMod>.Equals(CustomMod other)
		{
			return (Token == other.Token && Type == other.Type);
		}

		#endregion IEquatable<CustomMod> Members
	}
}