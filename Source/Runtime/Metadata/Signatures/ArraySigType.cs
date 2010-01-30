/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// Signature type of arrays.
    /// </summary>
    public sealed class ArraySigType : SigType
    {
        #region Data members

        /// <summary>
        /// The array element type signature.
        /// </summary>
        private SigType _type;

        /// <summary>
        /// Holds the rank of the array (number of dimensions)
        /// </summary>
        private int _rank;


        /// <summary>
        /// Holds the sizes of each rank of the array.
        /// </summary>
        private int[] _sizes;

        /// <summary>
        /// Holds the lower bound of each rank of the array.
        /// </summary>
        private int[] _lowbounds;

        #endregion // Data members

        #region Construction

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
            if (null == type)
                throw new ArgumentNullException(@"type");

            _type = type;
            _rank = rank;
            _sizes = sizes;
            _lowbounds = lowBounds;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public SigType ElementType { get { return _type; } }

        /// <summary>
        /// Gets the array rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get { return _rank; } }

        /// <summary>
        /// Gets the sizes of the array ranks.
        /// </summary>
        /// <value>The sizes.</value>
        public int[] Sizes { get { return _sizes; } }

        /// <summary>
        /// Gets the lower bounds of the array ranks.
        /// </summary>
        /// <value>The low bounds.</value>
        public int[] LowBounds { get { return _lowbounds; } }

        #endregion // Properties

        #region SigType Overrides

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals(SigType other)
        {
            ArraySigType ast = other as ArraySigType;
            if (null == ast)
                return false;

            return (base.Equals(other) && _rank == ast._rank && Equal(_sizes, ast._sizes) && Equal(_lowbounds, ast._lowbounds));
        }

        /// <summary>
        /// Compares the two arrays for equality.
        /// </summary>
        /// <param name="first">The first array to compare.</param>
        /// <param name="second">The second array to compare.</param>
        /// <returns>True if the arrays are equal.</returns>
        private static bool Equal(int[] first, int[] second)
        {
            if (first.Length != second.Length)
                return false;

            bool result = true;
            for (int idx = 0; idx < first.Length; idx++)
            {
                result = (first[idx] == second[idx]);
            }
            return result;
        }

        #endregion // SigType Overrides

        /// <summary>
        /// Expresses the array type reference in a meaningful, symbol-friendly string form
        /// </summary>
        /// <returns></returns>
        public override string ToSymbolPart()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ElementType.ToSymbolPart());
            sb.Append('[');
            // Don't write a comma for rank 1 on purpose...
            for (int x = 1; x < this.Rank; x++)
            {
                sb.Append(',');
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
