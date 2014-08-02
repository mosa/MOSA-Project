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
		public virtual IEnumerable<TypeInfo> DefinedTypes { get; protected set; }

		/// <summary>
		/// Gets the display name of the assembly.
		/// </summary>
		public virtual string FullName { get; protected set; }

		/// <summary>
		/// Gets a value indicating whether the assembly was loaded from the global assembly cache.
		/// </summary>
		public virtual bool GlobalAssemblyCache { get; protected set; }

		/// <summary>
		/// Gets the types defined in this assembly.
		/// </summary>
		/// <returns>An array that contains all the types that are defined in this assembly.</returns>
		public virtual Type[] GetTypes()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the public types defined in this assembly that are visible outside the assembly.
		/// </summary>
		/// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
		public virtual Type[] GetExportedTypes()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns information about the attributes that have been applied to the current <see cref="System.Reflection.Assembly">Assembly</see>, expressed as <see cref="System.Reflection.CustomAttributeData">CustomAttributeData</see> objects.
		/// </summary>
		/// <returns></returns>
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Locates the specified type from this assembly and creates an instance of it using the system activator, using case-sensitive search.
		/// </summary>
		/// <param name="typeName">The <see cref="System.Type">Type</see>.FullName of the type to locate.</param>
		/// <returns>An instance of the specified type created with the default constructor; or null if typeName is not found.</returns>
		public object CreateInstance(string typeName)
		{
			return this.CreateInstance(typeName, false);
		}

		/// <summary>
		/// Locates the specified type from this assembly and creates an instance of it using the system activator, with optional case-sensitive search.
		/// </summary>
		/// <param name="typeName">The <see cref="System.Type">Type</see>.FullName of the type to locate.</param>
		/// <param name="ignoreCase">True to ignore the case of the type name; otherwise, False.</param>
		/// <returns>An instance of the specified type created with the default constructor; or null if typeName is not found.</returns>
		public object CreateInstance(string typeName, bool ignoreCase)
		{
			Type t = Type.GetType(typeName, false, ignoreCase);
			if (t == null)
				return null;
			return Activator.CreateInstance(t);
		}

		/// <summary>
		/// Gets all the custom attributes for this assembly.
		/// </summary>
		/// <param name="inherit">This argument is ignored for objects of type Assembly.</param>
		/// <returns>An array that contains the custom attributes for this assembly.</returns>
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the custom attributes for this assembly as specified by type.
		/// </summary>
		/// <param name="attributeType">The type for which the custom attributes are to be returned.</param>
		/// <param name="inherit">This argument is ignored for objects of type Assembly.</param>
		/// <returns>An array that contains the custom attributes for this assembly as specified by attributeType.</returns>
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether this assembly and the specified object are equal.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns>True if o is equal to this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Assembly))
				return false;

			return ((Assembly)obj).FullName == this.FullName;
		}

		public override int GetHashCode()
		{
			return this.FullName.GetHashCode();
		}

		/// <summary>
		/// Indicates whether two <see cref="System.Reflection.Assembly">Assembly</see> objects are equal.
		/// </summary>
		/// <param name="left">The assembly to compare to right.</param>
		/// <param name="right">The assembly to compare to left.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(Assembly left, Assembly right)
		{
			if (object.ReferenceEquals(left, right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left.FullName == right.FullName;
		}

		/// <summary>
		/// Indicates whether two <see cref="System.Reflection.Assembly">Assembly</see> objects are not equal.
		/// </summary>
		/// <param name="left">The assembly to compare to right.</param>
		/// <param name="right">The assembly to compare to left.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(Assembly left, Assembly right)
		{
			return !(left == right);
		}
	}
}