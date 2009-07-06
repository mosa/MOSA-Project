/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
    /// <summary>
    /// Specifies the usage of another attribute class. This class cannot be inherited.
    /// </summary>
	//[Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AttributeUsageAttribute : Attribute
    {
		/// <summary>
		/// 
		/// </summary>
        public AttributeUsageAttribute(AttributeTargets validOn)
        {
        }


    }
}
