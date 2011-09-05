/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public class PropertySignature : Signature
	{
		/// <summary>
		/// PropertySignature signature is indexed by the Property.Type column
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void ParseSignature(SignatureReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
