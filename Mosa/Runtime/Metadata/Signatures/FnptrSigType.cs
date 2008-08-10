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
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
    public sealed class FnptrSigType : SigType
    {
        private TokenTypes _token;

        public FnptrSigType(TokenTypes token)
            : base(CilElementType.FunctionPtr)
        {
            _token = token;
        }

        public TokenTypes Token { get { return _token; } }
    }
}
