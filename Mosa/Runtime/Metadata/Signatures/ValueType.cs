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
    public sealed class ValueType : SigType
    {
        private TokenTypes _token;

        public ValueType(TokenTypes token)
            : base(CilElementType.ValueType)
        {
            _token = token;
        }

        public TokenTypes Token { get { return _token; } }
    }
}
