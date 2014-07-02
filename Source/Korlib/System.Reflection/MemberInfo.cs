/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Reflection
{
	/// <summary>
	/// Implementation of the "System.Reflection.MemberInfo" class
	/// </summary>
	[SerializableAttribute]
	public abstract class MemberInfo : ICustomAttributeProvider
	{
		/// <summary>
		/// A collection that contains this member's custom attributes.
		/// </summary>
		public virtual IEnumerable<CustomAttributeData> CustomAttributes { get; protected set; }

		/// <summary>
		/// Gets the class that declares this member.
		/// </summary>
		public abstract Type DeclaringType { get; }

		/// <summary>
		/// When overridden in a derived class, gets a <see cref="System.Reflection.MemberTypes">MemberTypes</see> value indicating the type of the member — method, constructor, event, and so on.
		/// </summary>
		public abstract MemberTypes MemberType { get; }

		/// <summary>
		/// Gets a value that identifies a metadata element.
		/// </summary>
		public virtual int MetadataToken
		{
			get { return 0; }
		}

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the class object that was used to obtain this instance of MemberInfo.
		/// </summary>
		public abstract Type ReflectedType { get; }

		protected MemberInfo() { }

		public abstract object[] GetCustomAttributes(bool inherit);

		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		public abstract bool IsDefined(Type attributeType, bool inherit);
	}
}