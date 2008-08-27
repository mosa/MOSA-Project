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
    public sealed class ArraySigType : SigType
    {
        private SigType _type;
        private int _rank;
        private int[] _sizes;
        private int[] _lowbounds;

        public ArraySigType(SigType type, int rank, int[] sizes, int[] lowBounds)
            : base(CilElementType.Array)
        {
            _type = type;
            _rank = rank;
            _sizes = sizes;
            _lowbounds = lowBounds;
        }

        public SigType ElementType { get { return _type; } }
        public int Rank { get { return _rank; } }
        public int[] Sizes { get { return _sizes; } }
        public int[] LowBounds { get { return _lowbounds; } }
    }
}
