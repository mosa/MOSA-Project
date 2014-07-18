/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Reflection
{
	/// <summary>
	/// Represents type declarations for class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.
	/// </summary>
	[Serializable]
	public abstract class TypeInfo : Type, IReflectableType
	{
		// TODO
		protected TypeInfo(RuntimeTypeHandle h)
			: base(h)
		{

		}

		TypeInfo IReflectableType.GetTypeInfo()
		{
			throw new NotImplementedException();
		}
	}
}