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
	/// 
	/// </summary>
	public class TypeSpecSignature : Signature
	{
		/// <summary>
		/// 
		/// </summary>
		private SigType type;

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType Type
		{
			get { return type; }
		}
				/// <summary>
		/// Initializes a new instance of the <see cref="TypeSpecSignature"/> class.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public TypeSpecSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeSpecSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public TypeSpecSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			type = SigType.ParseTypeSignature(reader);
		}
	}
}
