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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Represents type declarations: class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.
	/// </summary>
	[Serializable]
	public abstract class Type : MemberInfo
	{
		/// <summary>
		/// Separates names in the namespace of the Type. This field is read-only.
		/// </summary>
		public static readonly char Delimiter = '.';

		/// <summary>
		/// Represents an empty array of type Type. This field is read-only.
		/// </summary>
		public static readonly Type[] EmptyTypes = new Type[0];

		/// <summary>
		/// Represents a missing value in the Type information. This field is read-only.
		/// </summary>
		public static readonly object Missing;

		/// <summary>
		/// Gets the Assembly in which the type is declared. For generic types, gets the Assembly in which the generic type is defined.
		/// </summary>
		public abstract Assembly Assembly { get; }

		/// <summary>
		/// Gets the attributes associated with the Type.
		/// </summary>
		public TypeAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the type from which the current Type directly inherits.
		/// </summary>
		public abstract Type BaseType { get; }

		/// <summary>
		/// Gets a value indicating whether the current Type object has type parameters that have not been replaced by specific types.
		/// </summary>
		public virtual bool ContainsGenericParameters
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a MethodBase that represents the declaring method, if the current Type represents a type parameter of a generic method.
		/// </summary>
		public virtual MethodBase DeclaringMethod
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the type that declares the current nested type or generic type parameter.
		/// </summary>
		public override Type DeclaringType
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the fully qualified name of the Type, including the namespace of the Type but not the assembly.
		/// </summary>
		public abstract string Fullname { get; }

		/// <summary>
		/// Gets an array of the generic type arguments for this type.
		/// </summary>
		public virtual Type[] GenericTypeArguments
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the current Type encompasses or refers to another type; that is, whether the current Type is an array, a pointer, or is passed by reference.
		/// </summary>
		public bool HasElementType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is abstract and must be overridden.
		/// </summary>
		public bool IsAbstract
		{
			get { return (this.Attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract; }
		}

		/// <summary>
		/// Gets a value indicating whether the string format attribute AnsiClass is selected for the Type.
		/// </summary>
		public bool IsAnsiClass
		{
			get { return (this.Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.AnsiClass; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is an array.
		/// </summary>
		public bool IsArray
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the string format attribute AutoClass is selected for the Type.
		/// </summary>
		public bool IsAutoClass
		{
			get { return (this.Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass; }
		}

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out automatically by the common language runtime.
		/// </summary>
		public bool IsAutoLayout
		{
			get { return (this.Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is passed by reference.
		/// </summary>
		public bool IsByRef
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is a class; that is, not a value type or interface.
		/// </summary>
		public bool IsClass
		{
			get { return !(this.IsInterface || this.IsValueType); }
		}

		/// <summary>
		/// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
		/// </summary>
		public virtual bool IsConstructedGenericType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type can be hosted in a context.
		/// </summary>
		public bool IsContextful
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the current Type represents an enumeration.
		/// </summary>
		public virtual bool IsEnum
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out at explicitly specified offsets.
		/// </summary>
		public bool IsExplicitLayout
		{
			get { return (this.Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type has a ComImportAttribute attribute applied, indicating that it was imported from a COM type library.
		/// </summary>
		public bool IsImport
		{
			get { return (this.Attributes & TypeAttributes.Import) == TypeAttributes.Import; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is an interface; that is, not a class or a value type.
		/// </summary>
		public bool IsInterface
		{
			get { return (this.Attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface; }
		}

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out sequentially, in the order that they were defined or emitted to the metadata.
		/// </summary>
		public bool IsLayoutSequential
		{
			get { return (this.Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout; }
		}

		/// <summary>
		/// Gets a value indicating whether the current Type object represents a type whose definition is nested inside the definition of another type.
		/// </summary>
		public bool IsNested
		{
			get { return (Attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only within its own assembly.
		/// </summary>
		public bool IsNestedAssembly
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only to classes that belong to both its own family and its own assembly.
		/// </summary>
		public bool IsNestedFamANDAssem
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only within its own family.
		/// </summary>
		public bool IsNestedFamily
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only to classes that belong to either its own family or to its own assembly.
		/// </summary>
		public bool IsNestedFamORAssem
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is nested and declared private.
		/// </summary>
		public bool IsNestedPrivate
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate; }
		}

		/// <summary>
		/// Gets a value indicating whether a class is nested and declared public.
		/// </summary>
		public bool IsNestedPublic
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is not declared public.
		/// </summary>
		public bool IsNotPublic
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is a pointer.
		/// </summary>
		public bool IsPointer
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is one of the primitive types.
		/// </summary>
		public bool IsPrimitive
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is declared public.
		/// </summary>
		public bool IsPublic
		{
			get { return (this.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is declared sealed.
		/// </summary>
		public bool IsSealed
		{
			get { return (this.Attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is serializable.
		/// </summary>
		public virtual bool IsSerializable
		{
			get { return (this.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type has a name that requires special handling.
		/// </summary>
		public bool IsSpecialName
		{
			get { return (this.Attributes & TypeAttributes.SpecialName) == TypeAttributes.SpecialName; }
		}

		/// <summary>
		/// Gets a value indicating whether the string format attribute UnicodeClass is selected for the Type.
		/// </summary>
		public bool IsUnicodeClass
		{
			get { return (this.Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass; }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is a value type.
		/// </summary>
		public bool IsValueType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type can be accessed by code outside the assembly.
		/// </summary>
		public bool IsVisible
		{
			get { return (IsNestedPublic) ? DeclaringType.IsVisible : IsPublic; }
		}

		/// <summary>
		/// Gets a MemberTypes value indicating that this member is a type or a nested type.
		/// </summary>
		public override MemberTypes MemberType
		{
			get { return (this.IsNested) ? MemberTypes.NestedType : MemberTypes.TypeInfo; }
		}

		/// <summary>
		/// Gets the namespace of the Type.
		/// </summary>
		public abstract string Namespace { get; }

		/// <summary>
		/// Gets the class object that was used to obtain this member.
		/// </summary>
		public override Type ReflectedType
		{
			get { return null; }
		}

		/// <summary>
		/// Gets a StructLayoutAttribute that describes the layout of the current type.
		/// </summary>
		public virtual StructLayoutAttribute StructLayoutAttribute
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Initializes a new instance of the Type class.
		/// </summary>
		/// <param name="handle"></param>
		protected Type(RuntimeTypeHandle handle)
		{
		}

	}
}