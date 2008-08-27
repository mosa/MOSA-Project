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
    public sealed class GenericInstSigType : SigType
    {
        private SigType _baseType;
        private SigType[] _genericArgs;

        public GenericInstSigType(SigType baseType, SigType[] genericArgs)
            : base(CilElementType.GenericInst)
        {
            _baseType = baseType;
            _genericArgs = genericArgs;
        }

        public SigType BaseType { get { return _baseType; } }
        public SigType[] GenericArgs { get { return _genericArgs; } }
    }
}
