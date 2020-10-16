// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	/// <summary>
	/// Implementation of the "System.Reflection.DefaultMemberAttribute" class
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class DefaultMemberAttribute : Attribute
	{
		private readonly string member;

		/// <summary>
		/// Member
		/// </summary>
		public string Member
		{
			get { return member; }
		}

		/// <summary>
		///
		/// </summary>
		public DefaultMemberAttribute(string member)
		{
			this.member = member;
		}
	}
}
