/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	///
	/// </summary>
	public class StandAloneMethodSignature : MethodReferenceSignature
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StandAloneMethodSignature"/> class.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public StandAloneMethodSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StandAloneMethodSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public StandAloneMethodSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}
	}
}