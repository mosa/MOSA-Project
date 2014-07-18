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
	/// Discovers the attributes of a field and provides access to field metadata.
	/// </summary>
	[SerializableAttribute]
	public abstract class FieldInfo : MemberInfo
	{
		/// <summary>
		/// Gets the attributes associated with this field.
		/// </summary>
		public abstract FieldAttributes Attributes { get; }

		/// <summary>
		/// Gets a RuntimeFieldHandle, which is a handle to the internal metadata representation of a field.
		/// </summary>
		public abstract RuntimeFieldHandle FieldHandle { get; }

		/// <summary>
		/// Gets the type of this field object.
		/// </summary>
		public abstract Type FieldType { get; }

		/// <summary>
		/// Gets a value indicating whether the potential visibility of this field is described by FieldAttributes.Assembly; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
		/// </summary>
		public bool IsAssembly
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly; }
		}

		/// <summary>
		/// Gets a value indicating whether the visibility of this field is described by FieldAttributes.Family; that is, the field is visible only within its class and derived classes.
		/// </summary>
		public bool IsFamily
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family; }
		}

		/// <summary>
		/// Gets a value indicating whether the visibility of this field is described by FieldAttributes.FamANDAssem; that is, the field can be accessed from derived classes, but only if they are in the same assembly.
		/// </summary>
		public bool IsFamilyAndAssembly
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem; }
		}

		/// <summary>
		/// Gets a value indicating whether the potential visibility of this field is described by FieldAttributes.FamORAssem; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
		/// </summary>
		public bool IsFamilyOrAssembly
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem; }
		}

		/// <summary>
		/// Gets a value indicating whether the field can only be set in the body of the constructor.
		/// </summary>
		public bool IsInitOnly
		{
			get { return (this.Attributes & FieldAttributes.InitOnly) == FieldAttributes.InitOnly; }
		}

		/// <summary>
		/// Gets a value indicating whether the value is written at compile time and cannot be changed.
		/// </summary>
		public bool IsLiteral
		{
			get { return (this.Attributes & FieldAttributes.Literal) == FieldAttributes.Literal; }
		}

		/// <summary>
		/// Gets a value indicating whether this field has the NotSerialized attribute.
		/// </summary>
		public bool IsNotSerialized
		{
			get { return (this.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized; }
		}

		/// <summary>
		/// Gets a value indicating whether the corresponding PinvokeImpl attribute is set in FieldAttributes.
		/// </summary>
		public bool IsPinvokeImpl
		{
			get { return (this.Attributes & FieldAttributes.PinvokeImpl) == FieldAttributes.PinvokeImpl; }
		}

		/// <summary>
		/// Gets a value indicating whether the field is private.
		/// </summary>
		public bool IsPrivate
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private; }
		}

		/// <summary>
		/// Gets a value indicating whether the field is public.
		/// </summary>
		public bool IsPublic
		{
			get { return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public; }
		}

		/// <summary>
		/// Gets a value indicating whether the corresponding SpecialName attribute is set in the FieldAttributes enumerator.
		/// </summary>
		public bool IsSpecialName
		{
			get { return (this.Attributes & FieldAttributes.SpecialName) == FieldAttributes.SpecialName; }
		}

		/// <summary>
		/// Gets a value indicating whether the field is static.
		/// </summary>
		public bool IsStatic
		{
			get { return (this.Attributes & FieldAttributes.Static) == FieldAttributes.Static; }
		}

		/// <summary>
		/// Gets a MemberTypes value indicating that this member is a field.
		/// </summary>
		public override MemberTypes MemberType
		{
			get { return MemberTypes.Field; }
		}

		/// <summary>
		/// Initializes a new instance of the FieldInfo class.
		/// </summary>
		protected FieldInfo()
		{
		}

		/// <summary>
		/// Returns a value that indicates whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="o">An object to compare with this instance, or null.</param>
		/// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
		public override bool Equals(object o)
		{
			if (!(o is FieldInfo))
				return false;

			return ((FieldInfo)o).FieldHandle == this.FieldHandle;
		}

		/// <summary>
		/// Gets a FieldInfo for the field represented by the specified handle.
		/// </summary>
		/// <param name="handle">A RuntimeFieldHandle structure that contains the handle to the internal metadata representation of a field.</param>
		/// <returns>A FieldInfo object representing the field specified by handle.</returns>
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a FieldInfo for the field represented by the specified handle, for the specified generic type.
		/// </summary>
		/// <param name="handle">A RuntimeFieldHandle structure that contains the handle to the internal metadata representation of a field.</param>
		/// <param name="declaringType">A RuntimeTypeHandle structure that contains the handle to the generic type that defines the field.</param>
		/// <returns>A FieldInfo object representing the field specified by handle, in the generic type specified by declaringType.</returns>
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			// TODO
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a literal value associated with the field by a compiler.
		/// </summary>
		/// <returns>An Object that contains the literal value associated with the field. If the literal value is a class type with an element value of zero, the return value is null.</returns>
		public virtual object GetRawConstantValue()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// When overridden in a derived class, returns the value of a field supported by a given object.
		/// </summary>
		/// <param name="obj">The object whose field value will be returned.</param>
		/// <returns>An object containing the value of the field reflected by this instance.</returns>
		public abstract object GetValue(object obj);

		public void SetValue(object obj, object value)
		{
			// TODO: uses SetValue(object, object, BindingFlags, Binder, CultureInfo) but that currently isn't implemented
			throw new NotImplementedException();
		}

		/// <summary>
		/// When overridden in a derived class, sets the value of the field supported by the given object.
		/// </summary>
		/// <param name="obj">The object whose field value will be set.</param>
		/// <param name="value">The value to assign to the field.</param>
		/// <param name="invokeAttr">A field of Binder that specifies the type of binding that is desired (for example, Binder.CreateInstance or Binder.ExactBinding).</param>
		/// <param name="binder">A set of properties that enables the binding, coercion of argument types, and invocation of members through reflection. If binder is null, then Binder.DefaultBinding is used.</param>
		/// <param name="culture">The software preferences of a particular culture.</param>
		//public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)

		/// <summary>
		/// Indicates whether two FieldInfo objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(FieldInfo left, FieldInfo right)
		{
			if (object.ReferenceEquals(left, right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left.FieldHandle == right.FieldHandle;
		}

		/// <summary>
		/// Indicates whether two FieldInfo objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(FieldInfo left, FieldInfo right)
		{
			return !(left == right);
		}
	}
}