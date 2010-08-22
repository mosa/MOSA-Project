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

		protected VariableSignature()
		{
		}

		public VariableSignature(ISignatureContext context, byte[] buffer, ref int index)
		{
			this.ParseSignature(context, buffer, ref index);
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
		}

		protected override void ParseSignature(ISignatureContext context, byte[] buffer, ref int index)
		{
			this.ParseModifier(buffer, ref index);

			this.customMods = CustomMod.ParseCustomMods(buffer, ref index);
			this.type = SigType.ParseTypeSignature(context, buffer, ref index);
		}

		private void ParseModifier(byte[] buffer, ref int index)
		{
			CilElementType value = (CilElementType)buffer[index];
			if (value == CilElementType.Pinned)
			{
				this.modifier = value;
				index++;
			}
		}
	}
}
