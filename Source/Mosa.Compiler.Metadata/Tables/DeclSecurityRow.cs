/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Security.Permissions;

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class DeclSecurityRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DeclSecurityRow" /> struct.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="permissionSet">The permission set.</param>
		public DeclSecurityRow(SecurityAction action, Token parent, HeapIndexToken permissionSet)
		{
			Action = action;
			Parent = parent;
			PermissionSet = permissionSet;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>The action.</value>
		public SecurityAction Action { get; private set; }

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent { get; private set; }

		/// <summary>
		/// Gets the permission set.
		/// </summary>
		/// <value>The permission set.</value>
		public HeapIndexToken PermissionSet { get; private set; }

		#endregion Properties
	}
}