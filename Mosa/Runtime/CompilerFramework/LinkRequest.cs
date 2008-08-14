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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    public struct LinkRequest
    {
        #region Data members

        /// <summary>
        /// Holds the link request destination to patch up.
        /// </summary>
        private long _position;

        /// <summary>
        /// Holds the relative request flag.
        /// </summary>
        private long _relativeBase;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of LinkRequest.
        /// </summary>
        /// <param name="position">The position in code to patch for this request.</param>
        /// <param name="relativeBase">The base address for relative patching, otherwise zero.</param>
        public LinkRequest(long position, long relativeBase)
        {
            _position = position;
            _relativeBase = relativeBase;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Determines the relative base of the link request.
        /// </summary>
        public long RelativeBase
        {
            get { return _relativeBase; }
        }

        /// <summary>
        /// Returns the position of code to patch for this request.
        /// </summary>
        public long Position
        {
            get { return _position; }
        }

        #endregion // Properties
    }
}
