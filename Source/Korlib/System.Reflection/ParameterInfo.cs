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
	[Serializable]
	public class ParameterInfo : ICustomAttributeProvider
	{
		/// <summary>
		/// The attributes of the parameter.
		/// </summary>
		protected ParameterAttributes AttrsImpl;

		/// <summary>
		/// The <see cref="System.Type">Type</see> of the parameter.
		/// </summary>
		protected Type ClassImpl;

		/// <summary>
		/// The default value of the parameter.
		/// </summary>
		protected Object DefaultValueImpl;

		/// <summary>
		/// The member in which the field is implemented.
		/// </summary>
		protected MemberInfo MemberImpl;

		/// <summary>
		/// The name of the parameter.
		/// </summary>
		protected string NameImpl;

		/// <summary>
		/// The zero-based position of the parameter in the parameter list.
		/// </summary>
		protected int PositionImpl;

		/// <summary>
		/// Gets the attributes for this parameter.
		/// </summary>
		public virtual ParameterAttributes Attributes
		{
			get { return this.AttrsImpl; }
		}

		/// <summary>
		/// Gets a collection that contains this parameter's custom attributes.
		/// </summary>
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get { return this.GetCustomAttributesData(); }
		}

		/// <summary>
		/// Gets a value indicating the default value if the parameter has a default value.
		/// </summary>
		public virtual object DefaultValue
		{
			get { return this.DefaultValueImpl; }
		}

		/// <summary>
		/// Gets a value that indicates whether this parameter has a default value.
		/// </summary>
		public virtual bool HasDefaultValue
		{
			get { return (this.AttrsImpl & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault; }
		}

		/// <summary>
		/// Gets a value indicating whether this is an input parameter.
		/// </summary>
		public bool IsIn
		{
			get { return (this.AttrsImpl & ParameterAttributes.In) == ParameterAttributes.In; }
		}

		/// <summary>
		/// Gets a value indicating whether this parameter is optional.
		/// </summary>
		public bool IsOptional
		{
			get { return (this.AttrsImpl & ParameterAttributes.Optional) == ParameterAttributes.Optional; }
		}

		/// <summary>
		/// Gets a value indicating whether this is an output parameter.
		/// </summary>
		public bool IsOut
		{
			get { return (this.AttrsImpl & ParameterAttributes.Out) == ParameterAttributes.Out; }
		}

		/// <summary>
		/// Gets a value indicating the member in which the parameter is implemented.
		/// </summary>
		public virtual MemberInfo Member
		{
			get { return this.MemberImpl; }
		}

		/// <summary>
		/// Gets a value that identifies this parameter in metadata.
		/// </summary>
		public virtual int MetadataToken
		{
			get { return 0; }
		}

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		public virtual string Name
		{
			get { return this.NameImpl; }
		}

		/// <summary>
		/// Gets the <see cref="System.Type">Type</see> of this parameter.
		/// </summary>
		public virtual Type ParameterType
		{
			get { return this.ClassImpl; }
		}

		/// <summary>
		/// Gets the zero-based position of the parameter in the formal parameter list.
		/// </summary>
		public virtual int Position
		{
			get { return this.PositionImpl; }
		}

		/// <summary>
		/// Gets a value indicating the default value if the parameter has a default value.
		/// </summary>
		public virtual object RawDefaultValue
		{
			get { return this.DefaultValueImpl; }
		}

		/// <summary>
		/// Initializes a new instance of the ParameterInfo class.
		/// </summary>
		protected ParameterInfo()
		{

		}

		/// <summary>
		/// Gets all the custom attributes defined on this parameter.
		/// </summary>
		/// <param name="inherit">This argument is ignored for objects of this type.</param>
		/// <returns>An array that contains all the custom attributes applied to this parameter.</returns>
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the custom attributes of the specified type or its derived types that are applied to this parameter.
		/// </summary>
		/// <param name="attributeType">The custom attributes identified by type.</param>
		/// <param name="inherit">This argument is ignored for objects of this type.</param>
		/// <returns>An array that contains the custom attributes of the specified type or its derived types.</returns>
		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the custom attribute of the specified type or its derived types is applied to this parameter.
		/// </summary>
		/// <param name="attributeType">The Type object to search for.</param>
		/// <param name="inherit">This argument is ignored for objects of this type.</param>
		/// <returns>True if one or more instances of attributeType or its derived types are applied to this parameter; otherwise, False.</returns>
		public bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a list of CustomAttributeData objects for the current parameter, which can be used in the reflection-only context.
		/// </summary>
		/// <returns>A generic list of CustomAttributeData objects representing data about the attributes that have been applied to the current parameter.</returns>
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw new NotImplementedException();
		}
	}
}