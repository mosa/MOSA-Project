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
	public class DefaultSignatureContext : ISignatureContext
	{
		public static readonly ISignatureContext Instance = new DefaultSignatureContext();

		private DefaultSignatureContext()
		{
		}

		public SigType GetGenericMethodArgument(int index)
		{
			return new MVarSigType(index);
		}


		public SigType GetGenericTypeArgument(int index)
		{
			return new VarSigType(index);
		}
	}
}
