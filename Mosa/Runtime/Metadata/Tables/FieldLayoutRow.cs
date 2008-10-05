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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables 
{
    /// <summary>
    /// 
    /// </summary>
	public struct FieldLayoutRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private uint _offset;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _field;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldLayoutRow"/> struct.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="field">The field.</param>
        public FieldLayoutRow(uint offset, TokenTypes field)
        {
            _offset = offset;
            _field = field;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public uint Offset
        {
            get { return _offset; }
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>The field.</value>
        public TokenTypes Field
        {
            get { return _field; }
        }

        #endregion // Properties
	}
}
