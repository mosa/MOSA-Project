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
    public sealed class ArraySigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private SigType _type;
        /// <summary>
        /// 
        /// </summary>
        private int _rank;
        /// <summary>
        /// 
        /// </summary>
        private int[] _sizes;
        /// <summary>
        /// 
        /// </summary>
        private int[] _lowbounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArraySigType"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rank">The rank.</param>
        /// <param name="sizes">The sizes.</param>
        /// <param name="lowBounds">The low bounds.</param>
        public ArraySigType(SigType type, int rank, int[] sizes, int[] lowBounds)
            : base(CilElementType.Array)
        {
            _type = type;
            _rank = rank;
            _sizes = sizes;
            _lowbounds = lowBounds;
        }

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public SigType ElementType { get { return _type; } }
        /// <summary>
        /// Gets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get { return _rank; } }
        /// <summary>
        /// Gets the sizes.
        /// </summary>
        /// <value>The sizes.</value>
        public int[] Sizes { get { return _sizes; } }
        /// <summary>
        /// Gets the low bounds.
        /// </summary>
        /// <value>The low bounds.</value>
        public int[] LowBounds { get { return _lowbounds; } }
    }
}
