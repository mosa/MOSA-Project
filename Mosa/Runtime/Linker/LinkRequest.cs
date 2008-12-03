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
        private readonly RuntimeMethod method;

        /// <summary>
        /// The position within the code stream where the address is patched
        /// </summary>
        private readonly int methodOffset;

        /// <summary>
        /// Holds the relative request flag.
        /// </summary>
        private readonly int methodRelativeBase;

        /// <summary>
        /// The type of the link operation to perform.
        /// </summary>
        private readonly LinkType linkType;

        /// <summary>
        /// Holds the symbol name to link against.
        /// </summary>
        private readonly string symbolName;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of LinkRequest.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="method">The method whose code is being patched.</param>
        /// <param name="methodOffset">The method offset.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="symbolName">The linker symbol to link against.</param>
        public LinkRequest(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, string symbolName)
        {
            this.method = method;
            this.methodOffset = methodOffset;
            this.linkType = linkType;
            this.methodRelativeBase = methodRelativeBase;
            this.symbolName = symbolName;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// The method whose code is being patched.
        /// </summary>
        public RuntimeMethod Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Determines the relative base of the link request.
        /// </summary>
        public int MethodRelativeBase
        {
            get { return this.methodRelativeBase; }
        }

        /// <summary>
        /// The type of link required
        /// </summary>
        public LinkType LinkType
        {
            get { return this.linkType; }
        }

        /// <summary>
        /// The position within the code stream where the address is patched.
        /// </summary>
        public int MethodOffset
        {
            get { return this.methodOffset; }
        }

        /// <summary>
        /// Gets the name of the symbol.
        /// </summary>
        /// <value>The name of the symbol.</value>
        public string SymbolName
        {
            get { return this.symbolName; }
        }

        #endregion // Properties
    }
}
