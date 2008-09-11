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
    public class RefSigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private SigType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefSigType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RefSigType(SigType type)
            : base(CilElementType.ByRef)
        {
            _type = type;
        }

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public SigType ElementType { get { return _type; } }
    }
}
