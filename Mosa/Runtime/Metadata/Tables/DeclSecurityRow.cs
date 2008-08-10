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
using System.Security.Permissions;

namespace Mosa.Runtime.Metadata.Tables 
{
	public struct DeclSecurityRow 
    {
		#region Data members

        private SecurityAction _action;

        private TokenTypes _parentTableIdx;

        private TokenTypes _permissionSetBlobIdx;

		#endregion // Data members

        #region Construction

        public DeclSecurityRow(SecurityAction action, TokenTypes parentTableIdx, TokenTypes permissionSetBlobIdx)
        {
            _action = action;
            _parentTableIdx = parentTableIdx;
            _permissionSetBlobIdx = permissionSetBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public SecurityAction Action
        {
            get { return _action; }
        }

        public TokenTypes ParentTableIdx
        {
            get { return _parentTableIdx; }
        }

        public TokenTypes PermissionSetBlobIdx
        {
            get { return _permissionSetBlobIdx; }
        }

        #endregion // Properties
	}
}
