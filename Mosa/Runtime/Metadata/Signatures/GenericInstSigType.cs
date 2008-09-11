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
    public sealed class GenericInstSigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private SigType _baseType;
        /// <summary>
        /// 
        /// </summary>
        private SigType[] _genericArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericInstSigType"/> class.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="genericArgs">The generic args.</param>
        public GenericInstSigType(SigType baseType, SigType[] genericArgs)
            : base(CilElementType.GenericInst)
        {
            _baseType = baseType;
            _genericArgs = genericArgs;
        }

        /// <summary>
        /// Gets the type of the base.
        /// </summary>
        /// <value>The type of the base.</value>
        public SigType BaseType { get { return _baseType; } }
        /// <summary>
        /// Gets the generic args.
        /// </summary>
        /// <value>The generic args.</value>
        public SigType[] GenericArgs { get { return _genericArgs; } }
    }
}
