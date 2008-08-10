/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
    public abstract class Signature
    {
		public void LoadSignature(IMetadataProvider provider, TokenTypes token)
		{
			byte[] buffer;
			int index = 0;
			provider.Read(token, out buffer);
			ParseSignature(buffer, ref index);
			Debug.Assert(index == buffer.Length, @"Signature parser didn't complete.");
		}
		
		protected abstract void ParseSignature(byte[] buffer, ref int index);

        public static Signature FromMemberRefSignatureToken(IMetadataProvider provider, TokenTypes token)
        {
            Signature result = null;
            int index = 0;
            byte[] buffer;
            provider.Read(token, out buffer);

            if (0x06 == buffer[0])
            {
                result = new FieldSignature();
                result.ParseSignature(buffer, ref index);
            }
            else
            {
                result = new MethodSignature();
                result.ParseSignature(buffer, ref index);
            }
            Debug.Assert(index == buffer.Length, @"Not all signature bytes read.");
            return result;
        }
    }
}
