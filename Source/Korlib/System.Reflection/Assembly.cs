/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Reflection
{
	[Serializable]
	public abstract class Assembly : ICustomAttributeProvider
	{
		/// <summary>
		/// Gets a collection of the types defined in this assembly.
		/// </summary>
		public virtual IEnumerable<TypeInfo> DefinedTypes { get; private set; }

		/// <summary>
		/// Gets the display name of the assembly.
		/// </summary>
		public virtual string FullName { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the assembly was loaded from the global assembly cache.
		/// </summary>
		public virtual bool GlobalAssemblyCache { get; private set; }

		/// <summary>
		/// Gets the types defined in this assembly.
		/// </summary>
		/// <returns>An array that contains all the types that are defined in this assembly.</returns>
		public virtual Type[] GetTypes()
		{
			throw new NotImplementedException();
		}

		public virtual object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}
	}
}