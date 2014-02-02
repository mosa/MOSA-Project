/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

namespace System.Reflection
{
	/// <summary>
	/// Implementation of the "System.Reflection.DefaultMemberAttribute" class
	/// </summary>
	[SerializableAttribute]
	public abstract class MemberInfo : ICustomAttributeProvider
	{
		protected MemberInfo()
		{
		}

        public object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}