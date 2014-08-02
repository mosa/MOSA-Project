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
	/// Obtains information about the attributes of a member and provides access to member metadata.
	/// </summary>
	[SerializableAttribute]
	public abstract class MemberInfo : ICustomAttributeProvider
	{
		/// <summary>
		/// A collection that contains this member's custom attributes.
		/// </summary>
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get { return this.GetCustomAttributesData(); }
		}

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

		protected MemberInfo()
		{

		}

		/// <summary>
		/// When overridden in a derived class, returns an array of all custom attributes applied to this member.
		/// </summary>
		/// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, False. This parameter is ignored for properties and events.</param>
		/// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
		public abstract object[] GetCustomAttributes(bool inherit);

		/// <summary>
		/// When overridden in a derived class, returns an array of custom attributes applied to this member and identified by Type.
		/// </summary>
		/// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
		/// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, False. This parameter is ignored for properties and events.</param>
		/// <returns>An array of custom attributes applied to this member, or an array with zero elements if no attributes assignable to attributeType have been applied.</returns>
		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		/// <summary>
		/// When overridden in a derived class, indicates whether one or more attributes of the specified type or of its derived types is applied to this member.
		/// </summary>
		/// <param name="attributeType">The type of custom attribute to search for. The search includes derived types.</param>
		/// <param name="inherit">True to search this member's inheritance chain to find the attributes; otherwise, False. This parameter is ignored for properties and events.</param>
		/// <returns>True if one or more instances of attributeType or any of its derived types is applied to this member; otherwise, False.</returns>
		public abstract bool IsDefined(Type attributeType, bool inherit);

		/// <summary>
		/// Returns a list of CustomAttributeData objects representing data about the attributes that have been applied to the target member.
		/// </summary>
		/// <returns>A generic list of CustomAttributeData objects representing data about the attributes that have been applied to the target member.</returns>
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw new NotImplementedException();
		}
	}
}