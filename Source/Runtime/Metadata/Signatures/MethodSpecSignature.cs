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
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodSpecSignature : Signature, ISignatureContext
	{
		//private readonly ISignatureContext outerContext;

		/// <summary>
		/// 
		/// </summary>
		private SigType[] types;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSpecSignature"/> class.
		/// </summary>
		public MethodSpecSignature()
		{
			//this.outerContext = outerContext;
		}

		/// <summary>
		/// Gets the types.
		/// </summary>
		/// <value>The types.</value>
		public SigType[] Types
		{
			get { return types; }
		}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			if (reader.ReadByte() != 0x0A)
				throw new InvalidOperationException(@"Invalid signature.");

			int genArgCount = reader.ReadCompressedInt32();
			types = new SigType[genArgCount];
			for (int i = 0; i < genArgCount; i++)
				types[i] = SigType.ParseTypeSignature(reader);
		}

		public SigType GetGenericMethodArgument(int index)
		{
			return types[index];
		}

		public SigType GetGenericTypeArgument(int index)
		{
			return null; // TODO: Fixme!
			//return outerContext.GetGenericTypeArgument(index);
		}
	}
}
