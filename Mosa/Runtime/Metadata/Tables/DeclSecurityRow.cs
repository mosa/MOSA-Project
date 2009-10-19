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
using System.Security.Permissions;

namespace Mosa.Runtime.Metadata.Tables 
{
    /// <summary>
    /// 
    /// </summary>
	public struct DeclSecurityRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private SecurityAction _action;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parentTableIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _permissionSetBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DeclSecurityRow"/> struct.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parentTableIdx">The parent table idx.</param>
        /// <param name="permissionSetBlobIdx">The permission set BLOB idx.</param>
        public DeclSecurityRow(SecurityAction action, TokenTypes parentTableIdx, TokenTypes permissionSetBlobIdx)
        {
            _action = action;
            _parentTableIdx = parentTableIdx;
            _permissionSetBlobIdx = permissionSetBlobIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public SecurityAction Action
        {
            get { return _action; }
        }

        /// <summary>
        /// Gets the parent table idx.
        /// </summary>
        /// <value>The parent table idx.</value>
        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }

        /// <summary>
        /// Gets the permission set BLOB idx.
        /// </summary>
        /// <value>The permission set BLOB idx.</value>
        public TokenTypes PermissionSetBlobIdx
        {
            get { return _permissionSetBlobIdx; }
        }

        #endregion // Properties
	}
}
