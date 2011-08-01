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
	public class VariableSignature : Signature
	{
		private CustomMod[] customMods;
		private CilElementType modifier;
		private SigType type;

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
			this.customMods = signature.customMods;
			this.modifier = signature.modifier;
			this.type = signature.type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="signature">The signature.</param>
		/// <param name="genericArguments">The generic arguments.</param>
		public VariableSignature(VariableSignature signature, SigType[] genericArguments)
			: this(signature)
		{
			ApplyGenericArguments(genericArguments);
		}

		/// <summary>
		/// Gets the custom mods.
		/// </summary>
		/// <value>The custom mods.</value>
		public CustomMod[] CustomMods
		{
			get { return this.customMods; }
		}

		public CilElementType Modifier
		{
			get { return this.modifier; }
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		protected override void ParseSignature(SignatureReader reader)
		{
			this.ParseModifier(reader);

			this.customMods = CustomMod.ParseCustomMods(reader);
			this.type = SigType.ParseTypeSignature(reader);
		}

		private void ParseModifier(SignatureReader reader)
		{
			CilElementType value = (CilElementType)reader.PeekByte();
			if (value == CilElementType.Pinned)
			{
				this.modifier = value;
				reader.SkipByte();
			}
		}

		protected void ApplyGenericArguments(SigType[] genericArguments)
		{
			if (genericArguments == null)
				return;

			if (this.Type is VarSigType)
			{
				if ((Type as VarSigType).Index < genericArguments.Length)
					this.Type = genericArguments[(Type as VarSigType).Index];
			}
			else if (this.Type is GenericInstSigType)
			{
				var genericInstSigType = this.Type as GenericInstSigType;
				for (var i = 0; i < genericInstSigType.GenericArguments.Length; ++i)
				{
					if (genericInstSigType.GenericArguments[i] is VarSigType)
						genericInstSigType.GenericArguments[i] = genericArguments[(genericInstSigType.GenericArguments[i] as VarSigType).Index];
				}
			}
		}

	}
}
