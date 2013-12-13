/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	///
	/// </summary>
	public abstract class Signature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Signature"/> class.
		/// </summary>
		protected Signature()
		{
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
		public Signature(IMetadataProvider provider, HeapIndexToken token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));
			ParseSignature(reader);
			Debug.Assert(reader.Index == reader.Length, "Signature parser didn't complete.");
		}

		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public void LoadSignature(IMetadataProvider provider, HeapIndexToken token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));
			ParseSignature(reader);
			Debug.Assert(reader.Index == reader.Length, "Signature parser didn't complete.");
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected abstract void ParseSignature(SignatureReader reader);

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return "[" + GetType().Name + "]";
		}

		/// <summary>
		/// Gets the signature from member reference.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public static Signature GetSignatureFromMemberRef(IMetadataProvider provider, HeapIndexToken token)
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

		/// <summary>
		/// Gets the signature from stand along sig.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public static Signature GetSignatureFromStandAlongSig(IMetadataProvider provider, HeapIndexToken token)
		{
			SignatureReader reader = new SignatureReader(provider.ReadBlob(token));

			if (reader[0] == 0x07)
			{
				return new LocalVariableSignature(reader);
			}
			else
			{
				return new StandAloneMethodSignature(reader);
			}
		}
	}
}