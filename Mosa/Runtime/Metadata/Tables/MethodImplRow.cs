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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
    /// <summary>
    /// 
    /// </summary>
	public struct MethodImplRow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _classTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _methodBodyTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _methodDeclarationTableIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodImplRow"/> struct.
        /// </summary>
        /// <param name="classTableIdx">The class table idx.</param>
        /// <param name="methodBodyTableIdx">The method body table idx.</param>
        /// <param name="methodDeclarationTableIdx">The method declaration table idx.</param>
        public MethodImplRow(TokenTypes classTableIdx, TokenTypes methodBodyTableIdx, TokenTypes methodDeclarationTableIdx)
        {
            _classTableIdx = classTableIdx;
            _methodBodyTableIdx = methodBodyTableIdx;
            _methodDeclarationTableIdx = methodDeclarationTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the class table idx.
        /// </summary>
        /// <value>The class table idx.</value>
        public TokenTypes ClassTableIdx
        {
            get { return _classTableIdx; }
        }

        /// <summary>
        /// Gets the method body table idx.
        /// </summary>
        /// <value>The method body table idx.</value>
        public TokenTypes MethodBodyTableIdx
        {
            get { return _methodBodyTableIdx; }
        }

        /// <summary>
        /// Gets the method declaration table idx.
        /// </summary>
        /// <value>The method declaration table idx.</value>
        public TokenTypes MethodDeclarationTableIdx
        {
            get { return _methodDeclarationTableIdx; }
        }

        #endregion // Properties
	}
}
