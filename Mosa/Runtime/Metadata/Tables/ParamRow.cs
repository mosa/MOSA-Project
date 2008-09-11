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
	public struct ParamRow 
    {
		#region Data members

        /// <summary>
        /// Holds the flags of the parameter.
        /// </summary>
		private ParameterAttributes _flags;

        /// <summary>
        /// The token holding the name of the parameter.
        /// </summary>
        private TokenTypes _nameIdx;

        /// <summary>
        /// Holds the sequence index of the parameter.
        /// </summary>
		private short _sequence;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamRow"/> struct.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="nameIdx">The name idx.</param>
        public ParamRow(ParameterAttributes flags, short sequence, TokenTypes nameIdx)
        {
            _nameIdx = nameIdx;
            _sequence = sequence;
            _flags = flags;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the attributes of this parameter.
        /// </summary>
        /// <value>The flags.</value>
        public ParameterAttributes Flags
        {
            get { return _flags; }
        }

        /// <summary>
        /// Retrieves the token of the parameter name.
        /// </summary>
        /// <value>The name idx.</value>
        public TokenTypes NameIdx
        {
            get
            {
                return _nameIdx;
            }
        }

        /// <summary>
        /// Retrieves the parameter sequence number.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence
        {
            get { return _sequence; }
        }

        #endregion // Properties
	}
}
