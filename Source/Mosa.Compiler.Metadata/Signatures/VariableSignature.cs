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
	public class VariableSignature : Signature
	{
		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType Type { get; private set; }

		/// <summary>
		/// Gets the custom mods.
		/// </summary>
		/// <value>The custom mods.</value>
		public CustomMod[] CustomMods { get; private set; }

		/// <summary>
		/// Gets the modifier.
		/// </summary>
		/// <value>
		/// The modifier.
		/// </value>
		public CilElementType Modifier { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public VariableSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public VariableSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

		protected override void ParseSignature(SignatureReader reader)
		{
			ParseModifier(reader);
			CustomMods = CustomMod.ParseCustomMods(reader);
			Type = SigType.ParseTypeSignature(reader);
		}

		/// <summary>
		/// Parses the modifier.
		/// </summary>
		/// <param name="reader">The reader.</param>
		private void ParseModifier(SignatureReader reader)
		{
			CilElementType value = (CilElementType)reader.PeekByte();
			if (value == CilElementType.Pinned)
			{
				Modifier = value;
				reader.SkipByte();
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(base.ToString() + " ");
			sb.Append(Type.ToString());
			//sb.Append(" Modifier: ");
			//sb.Append(Modifier.ToString());
			return sb.ToString();
		}
	}
}