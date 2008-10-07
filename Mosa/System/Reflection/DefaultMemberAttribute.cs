/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Reflection
{
	/// <summary>
	/// Implementation of the "System.Reflection.DefaultMemberAttribute" class
	/// </summary>
	public class DefaultMemberAttribute : Attribute
	{
		private readonly string member;

		public string Member
		{
			get { return member; }
		}

		public DefaultMemberAttribute(string member) { this.member = member; }
	}
}
