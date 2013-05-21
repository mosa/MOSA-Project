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
	public class MethodReferenceSignature : MethodSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodReferenceSignature"/> class.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public MethodReferenceSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodReferenceSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public MethodReferenceSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}
	}
}