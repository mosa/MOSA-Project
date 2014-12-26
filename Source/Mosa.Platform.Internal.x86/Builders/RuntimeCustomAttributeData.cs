/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Reflection;

namespace System
{
	public sealed unsafe class RuntimeCustomAttributeData : CustomAttributeData
	{
		public RuntimeCustomAttributeData(uint* customAttributeTable)
		{
			RuntimeTypeHandle typeHandle = new RuntimeTypeHandle();
			((uint**)&typeHandle)[0] = (uint*)customAttributeTable[0];
			base.attributeType = Type.GetTypeFromHandle(typeHandle);
		}
	}
}
