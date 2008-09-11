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
    public sealed class VarSigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarSigType"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public VarSigType(int index)
            : base(CilElementType.Var)
        {
            _index = index;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get { return _index; } }
    }
}
