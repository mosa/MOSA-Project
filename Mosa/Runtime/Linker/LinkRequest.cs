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
using System.IO;

namespace Mosa.Runtime.Linker
{
    /// <summary>
    /// Represents a linking request to the assembly linker.
    /// </summary>
    public struct LinkRequest
    {
        #region Data members

        /// <summary>
        /// The method whose code is being patched.
        /// </summary>
        RuntimeMethod _method;

        /// <summary>
        /// The position within the code stream where the address is patched
        /// </summary>
        private int _methodOffset;

        /// <summary>
        /// Holds the relative request flag.
        /// </summary>
        private int _methodRelativeBase;

        /// <summary>
        /// 
        /// </summary>
        private LinkType _linkType;

        /// <summary>
        /// The symbolic target of this link request
        /// </summary>
        private string symbol;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of LinkRequest.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The method offset.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="symbol">The symbol.</param>
        public LinkRequest(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, string symbol)
        {
            _method = method;
            _methodOffset = methodOffset;
            _linkType = linkType;
            _methodRelativeBase = methodRelativeBase;
            this.symbol = symbol;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// The method whose code is being patched.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Determines the relative base of the link request.
        /// </summary>
        public int MethodRelativeBase
        {
            get { return _methodRelativeBase; }
        }

        /// <summary>
        /// The type of link required
        /// </summary>
        public LinkType LinkType
        {
            get { return _linkType; }
        }

        /// <summary>
        /// The position within the code stream where the address is patched.
        /// </summary>
        public int MethodOffset
        {
            get { return _methodOffset; }
        }

        /// <summary>
        /// The target of this link
        /// </summary>
        public string Symbol
        {
            get { return this.symbol; }
        }

        #endregion // Properties
    }
}
