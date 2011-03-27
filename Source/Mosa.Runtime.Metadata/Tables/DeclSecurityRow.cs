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
		private SecurityAction action;

		/// <summary>
		/// 
		/// </summary>
		private Token parent;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken permissionSet;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclSecurityRow"/> struct.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="parent">The parent table idx.</param>
		/// <param name="permissionSet">The permission set BLOB idx.</param>
		public DeclSecurityRow(SecurityAction action, Token parent, HeapIndexToken permissionSet)
		{
			this.action = action;
			this.parent = parent;
			this.permissionSet = permissionSet;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>The action.</value>
		public SecurityAction Action
		{
			get { return action; }
		}

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the permission set.
		/// </summary>
		/// <value>The permission set.</value>
		public HeapIndexToken PermissionSet
		{
			get { return permissionSet; }
		}

		#endregion // Properties
	}
}
