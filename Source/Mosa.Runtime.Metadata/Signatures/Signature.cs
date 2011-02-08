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
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Signature
	{
		/// <summary>
		/// 
		/// </summary>
		private TokenTypes token;

		/// <summary>
		/// Gets the token.
		/// </summary>
		/// <value>The token.</value>
		public TokenTypes Token
		{
			get { return token; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Signature"/> class.
		/// </summary>
		public Signature()
		{
			// TODO: Remove this constructor
		}

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public Signature(SignatureReader reader)
		{
			ParseSignature(reader);
		}

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public Signature(IMetadataProvider provider, TokenTypes token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));

			this.ParseSignature(reader);
			Debug.Assert(reader.Index == reader.Length, @"Signature parser didn't complete.");

			this.token = token;
		}

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="signature">The signature.</param>
		public Signature(Signature signature)
		{
			if (signature == null)
				throw new ArgumentNullException(@"signature");

			this.token = signature.token;
		}

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public void LoadSignature(IMetadataProvider provider, TokenTypes token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));

			this.ParseSignature(reader);
			Debug.Assert(reader.Index == reader.Length, @"Signature parser didn't complete.");

			this.token = token;
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected abstract void ParseSignature(SignatureReader reader);

		/// <summary>
		/// Froms the member ref signature token.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public static Signature FromMemberRefSignatureToken(IMetadataProvider provider, TokenTypes token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));

			if (reader[0] == 0x06)
			{
				return new FieldSignature(reader);
			}
			else
			{
				return new MethodSignature(reader);
			}

		}
	}
}
