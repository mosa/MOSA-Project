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
using System.Globalization;

namespace System.Reflection
{
	/// <summary>
	/// Obtains information about the attributes of a member and provides access to member metadata.
	/// </summary>
	[SerializableAttribute]
	public abstract class PropertyInfo : MemberInfo
	{
		/// <summary>
		/// Gets the attributes for this property.
		/// </summary>
		public abstract PropertyAttributes Attributes { get; }

		/// <summary>
		/// Gets a value indicating whether the property can be read.
		/// </summary>
		public abstract bool CanRead { get; }

		/// <summary>
		/// Gets a value indicating whether the property can be written to.
		/// </summary>
		public abstract bool CanWrite { get; }

		/// <summary>
		/// Gets the get accessor for this property.
		/// </summary>
		public virtual MethodInfo GetMethod
		{
			get { return this.GetGetMethod(); }
		}

		/// <summary>
		/// Gets a value indicating whether the property is the special name.
		/// </summary>
		public bool IsSpecialName
		{
			get { return (this.Attributes & PropertyAttributes.SpecialName) == PropertyAttributes.SpecialName; }
		}

		/// <summary>
		/// Gets a MemberTypes value indicating that this member is a property.
		/// </summary>
		public override MemberTypes MemberType
		{
			get { return MemberTypes.Property; }
		}

		/// <summary>
		/// Gets the type of this property.
		/// </summary>
		public abstract Type PropertyType { get; }

		/// <summary>
		/// Gets the set accessor for this property.
		/// </summary>
		public virtual MethodInfo SetMethod
		{
			get { return this.GetSetMethod(); }
		}

		/// <summary>
		/// Initializes a new instance of the PropertyInfo class.
		/// </summary>
		protected PropertyInfo()
		{
		}

		/// <summary>
		/// Returns a value that indicates whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance, or null.</param>
		/// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns an array whose elements reflect the public get, set, and other accessors of the property reflected by the current instance.
		/// </summary>
		/// <returns>An array of MethodInfo objects that reflect the public get, set, and other accessors of the property reflected by the current instance, if found; otherwise, this method returns an array with zero (0) elements.</returns>
		public MethodInfo[] GetAccessors()
		{
			return this.GetAccessors(false);
		}

		/// <summary>
		/// Returns an array whose elements reflect the public and, if specified, non-public get, set, and other accessors of the property reflected by the current instance.
		/// </summary>
		/// <param name="nonPublic">Indicates whether non-public methods should be returned in the MethodInfo array. True if non-public methods are to be included; otherwise, False.</param>
		/// <returns>
		/// An array of MethodInfo objects whose elements reflect the get, set, and other accessors of the property reflected by the current instance.
		/// If nonPublic is True, this array contains public and non-public get, set, and other accessors.
		/// If nonPublic is False, this array contains only public get, set, and other accessors.
		/// If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.
		/// </returns>
		public abstract MethodInfo[] GetAccessors(bool nonPublic);

		/// <summary>
		/// Returns a literal value associated with the property by a compiler.
		/// </summary>
		/// <returns>An Object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
		public virtual object GetConstantValue()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the public get accessor for this property.
		/// </summary>
		/// <returns>A MethodInfo object representing the public get accessor for this property, or null if the get accessor is non-public or does not exist.</returns>
		public MethodInfo GetGetMethod()
		{
			return GetGetMethod(false);
		}

		/// <summary>
		/// When overridden in a derived class, returns the public or non-public get accessor for this property.
		/// </summary>
		/// <param name="nonPublic">Indicates whether a non-public get accessor should be returned. True if a non-public accessor is to be returned; otherwise, False.</param>
		/// <returns>
		/// A MethodInfo object representing the get accessor for this property, if nonPublic is True.
		/// Returns null if nonPublic is False and the get accessor is non-public, or if nonPublic is True but no get accessors exist.
		/// </returns>
		public abstract MethodInfo GetGetMethod(bool nonPublic);

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// When overridden in a derived class, returns an array of all the index parameters for the property.
		/// </summary>
		/// <returns>An array of type ParameterInfo containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
		public abstract ParameterInfo[] GetIndexParameters();

		/// <summary>
		/// Returns an array of types representing the optional custom modifiers of the property.
		/// </summary>
		/// <returns>An array of Type objects that identify the optional custom modifiers of the current property, such as IsConst or IsImplicitlyDereferenced.</returns>
		public virtual Type[] GetOptionalCustomModifiers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a literal value associated with the property by a compiler.
		/// </summary>
		/// <returns>An Object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
		public virtual object GetRawConstantValue()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an array of types representing the required custom modifiers of the property.
		/// </summary>
		/// <returns>An array of Type objects that identify the required custom modifiers of the current property, such as IsConst or IsImplicitlyDereferenced.</returns>
		public virtual Type[] GetRequiredCustomModifiers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the public set accessor for this property.
		/// </summary>
		/// <returns>The MethodInfo object representing the Set method for this property if the set accessor is public, or null if the set accessor is not public.</returns>
		public MethodInfo GetSetMethod()
		{
			return this.GetSetMethod(false);
		}

		/// <summary>
		/// When overridden in a derived class, returns the set accessor for this property.
		/// </summary>
		/// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public. True if a non-public accessor is to be returned; otherwise, False.</param>
		/// <returns>
		/// A MethodInfo object representing the set accessor for this property, if nonPublic is True and set accessor is non-public or if set accessor is public.
		/// Returns null if nonPublic is False and the set accessor is non-public, or if nonPublic is True but no set accessors exist.
		/// </returns>
		public abstract MethodInfo GetSetMethod(bool nonPublic);

		/// <summary>
		/// Returns the property value of a specified object.
		/// </summary>
		/// <param name="obj">The object whose property value will be returned.</param>
		/// <returns>The property value of the specified object.</returns>
		public object GetValue (object obj)
		{
			return this.GetValue(obj, BindingFlags.Default, null, null, null);
		}

		/// <summary>
		/// Returns the property value of a specified object with optional index values for indexed properties.
		/// </summary>
		/// <param name="obj">The object whose property value will be returned.</param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
		/// <returns>The property value of the specified object.</returns>
		public virtual object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		/// <summary>
		/// When overridden in a derived class, returns the property value of a specified object that has the specified binding, index, and culture-specific information.
		/// </summary>
		/// <param name="obj">The object whose property value will be returned.</param>
		/// <param name="invokeAttr">
		/// A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, and SetProperty.
		/// You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.
		/// </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects through reflection. If binder is null, the default binder is used.</param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
		/// <param name="culture">
		/// The culture for which the resource is to be localized. If the resource is not localized for this culture, the CultureInfo.Parent property will be called successively in search of a match.
		/// If this value is null, the culture-specific information is obtained from the CultureInfo.CurrentUICulture property.
		/// </param>
		/// <returns>The property value of the specified object.</returns>
		public abstract object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		/// <summary>
		/// Sets the property value of a specified object.
		/// </summary>
		/// <param name="obj">The object whose property value will be set.</param>
		/// <param name="value">The new property value.</param>
		public void SetValue (object obj, object value)
		{
			this.SetValue (obj, value, BindingFlags.Default, null, null, null);
		}

		/// <summary>
		/// Sets the property value of a specified object with optional index values for index properties.
		/// </summary>
		/// <param name="obj">The object whose property value will be set.</param>
		/// <param name="value">The new property value.</param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
		public virtual void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Default, null, index, null);
		}

		/// <summary>
		/// When overridden in a derived class, sets the property value for a specified object that has the specified binding, index, and culture-specific information.
		/// </summary>
		/// <param name="obj">The object whose property value will be set.</param>
		/// <param name="value">The new property value.</param>
		/// <param name="invokeAttr">
		/// A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty.
		/// You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.
		/// </param>
		/// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects through reflection. If binder is null, the default binder is used.</param>
		/// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
		/// <param name="culture">
		/// The culture for which the resource is to be localized. If the resource is not localized for this culture, the CultureInfo.Parent property will be called successively in search of a match.
		/// If this value is null, the culture-specific information is obtained from the CultureInfo.CurrentUICulture property.
		/// </param>
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		/// <summary>
		/// Indicates whether two PropertyInfo objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(PropertyInfo left, PropertyInfo right)
		{
			if (object.ReferenceEquals(left, right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left.Equals(right);
		}

		/// <summary>
		/// Indicates whether two PropertyInfo objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(PropertyInfo left, PropertyInfo right)
		{
			return !(left == right);
		}
	}
}