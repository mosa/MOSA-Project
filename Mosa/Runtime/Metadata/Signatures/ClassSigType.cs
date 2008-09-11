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
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClassSigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassSigType"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ClassSigType(TokenTypes token)
            : base(CilElementType.Class)
        {
            _token = token;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        public TokenTypes Token { get { return _token; } }
    }
}
