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
	public class VariableSignature : Signature
	{
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

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public VariableSignature(VariableSignature signature)
			: base(signature)
		{
			CustomMods = signature.CustomMods;
			Modifier = signature.Modifier;
			Type = signature.Type;
		}

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
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType Type { get; private set; }

		protected override void ParseSignature(SignatureReader reader)
		{
			this.ParseModifier(reader);

			CustomMods = CustomMod.ParseCustomMods(reader);
			Type = SigType.ParseTypeSignature(reader);
		}

		private void ParseModifier(SignatureReader reader)
		{
			CilElementType value = (CilElementType)reader.PeekByte();
			if (value == CilElementType.Pinned)
			{
				Modifier = value;
				reader.SkipByte();
			}
		}
	}
}