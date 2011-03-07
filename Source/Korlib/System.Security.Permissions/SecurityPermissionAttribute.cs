/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Security.Permissions
{
	public class SecurityPermissionAttribute : Attribute
	{
		public SecurityPermissionAttribute(SecurityAction action)
		{
		}

		public bool SkipVerification
		{
			get { return true; }
			set { }
		}
	}
}