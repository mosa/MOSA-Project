/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Reflection;
using Mosa.Platform.Internal.x86;

namespace System
{
	public sealed unsafe class RuntimeCustomAttributeData : CustomAttributeData
	{
		public RuntimeCustomAttributeData(MetadataCAStruct* customAttributeTable)
		{
			RuntimeTypeHandle typeHandle = new RuntimeTypeHandle();
			((uint**)&typeHandle)[0] = (uint*)customAttributeTable->AttributeType;
			base.attributeType = Type.GetTypeFromHandle(typeHandle);
		}
	}
}
