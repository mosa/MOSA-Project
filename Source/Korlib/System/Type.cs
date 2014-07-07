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
		internal const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

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
		public static readonly object Missing = null;

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
		/// Gets the handle for the current Type.
		/// </summary>
		public virtual RuntimeTypeHandle TypeHandle
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the initializer for the Type.
		/// </summary>
		public ConstructorInfo TypeInitializer
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Initializes a new instance of the Type class.
		/// </summary>
		/// <param name="handle"></param>
		protected Type(RuntimeTypeHandle handle)
		{
		}

		/// <summary>
		/// Determines if the underlying system type of the current Type is the same as the underlying system type of the specified Object.
		/// </summary>
		/// <param name="o">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
		/// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False. This method also returns False if the object specified by the o parameter is not a Type.</returns>
		public override bool Equals(object o)
		{
			if (!(o is Type))
				return false;

			return ((Type)o).TypeHandle == this.TypeHandle;
		}

		/// <summary>
		/// Determines if the underlying system type of the current Type is the same as the underlying system type of the specified Type.
		/// </summary>
		/// <param name="o">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
		/// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False.</returns>
		public virtual bool Equals(Type o)
		{
			return o.TypeHandle == this.TypeHandle;
		}

		/// <summary>
		/// Gets the number of dimensions in an Array.
		/// </summary>
		/// <returns>An Int32 containing the number of dimensions in the current Type.</returns>
		public virtual int GetArrayRank()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// When overridden in a derived class, implements the Attributes property and gets a bitmask indicating the attributes associated with the Type.
		/// </summary>
		/// <returns>A TypeAttributes object representing the attribute set of the Type.</returns>
		protected abstract TypeAttributes GetAttributeFlagsImpl();

		/// <summary>
		/// Searches for a public instance constructor whose parameters match the types in the specified array.
		/// </summary>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the desired constructor.
		/// -or-
		/// An empty array of Type objects, to get a constructor that takes no parameters. Such an empty array is provided by the static field Type.EmptyTypes.
		/// </param>
		/// <returns>An object representing the public instance constructor whose parameters match the types in the parameter type array, if found; otherwise, null.</returns>
		public ConstructorInfo GetConstructor(Type[] types)
		{
			if (types == null)
				throw new ArgumentNullException("types");

			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
					throw new ArgumentNullException("types");
			}

			if (types.Rank > 1)
				throw new ArgumentException("types is multidimensional.", "types");

			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns all the public constructors defined for the current Type.
		/// </summary>
		/// <returns>An array of ConstructorInfo objects representing all the public instance constructors defined for the current Type, but not including the type initializer (static constructor). If no public instance constructors are defined for the current Type, or if the current Type represents a type parameter in the definition of a generic type or generic method, an empty array of type ConstructorInfo is returned.</returns>
		public ConstructorInfo[] GetConstructors()
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Searches for the members defined for the current Type whose DefaultMemberAttribute is set.
		/// </summary>
		/// <returns>
		/// An array of MemberInfo objects representing all default members of the current Type.
		/// -or- 
		/// An empty array of type MemberInfo, if the current Type does not have default members.
		/// </returns>
		public virtual MemberInfo[] GetDefaultMembers()
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// When overridden in a derived class, returns the Type of the object encompassed or referred to by the current array, pointer or reference type.
		/// </summary>
		/// <returns>The Type of the object encompassed or referred to by the current array, pointer, or reference type, or null if the current Type is not an array or a pointer, or is not passed by reference, or represents a generic type or a type parameter in the definition of a generic type or generic method.</returns>
		public abstract Type GetElementType();

		/// <summary>
		/// Returns the name of the constant that has the specified value, for the current enumeration type.
		/// </summary>
		/// <param name="value">The value whose name is to be retrieved.</param>
		/// <returns>The name of the member of the current enumeration type that has the specified value, or null if no such constant is found.</returns>
		public virtual string GetEnumName(object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			Type valueType = value.GetType();

			if (!valueType.IsEnum)
				throw new ArgumentException("The current type is not an enumeration.", "value");

			if (!this.IsInstanceOfType(value))
				throw new ArgumentException("value is neither of the current type nor does it have the same underlying type as the current type.", "value");

			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the names of the members of the current enumeration type.
		/// </summary>
		/// <returns>An array that contains the names of the members of the enumeration.</returns>
		public virtual string[] GetEnumNames()
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an array of the values of the constants in the current enumeration type.
		/// </summary>
		/// <returns>An array that contains the values. The elements of the array are sorted by the binary values (that is, the unsigned values) of the enumeration constants.</returns>
		public virtual Array GetEnumValues()
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Searches for the public field with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the data field to get.</param>
		/// <returns>An object representing the public field with the specified name, if found; otherwise, null.</returns>
		public FieldInfo GetField(string name)
		{
			return this.GetField(name, DefaultBindingFlags);
		}

		/// <summary>
		/// Searches for the specified field, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the data field to get.</param>
		/// <param name="bindingAttr"></param>
		/// <returns>
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </returns>
		public abstract FieldInfo GetField(string name, BindingFlags bindingAttr);
	}
}