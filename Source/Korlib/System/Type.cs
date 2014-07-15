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
using System.Globalization;

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
			get { return this.GetAttributeFlagsImpl(); }
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
			get { return null; }
		}

		/// <summary>
		/// Gets the type that declares the current nested type or generic type parameter.
		/// </summary>
		public override Type DeclaringType
		{
			get { return null; }
		}

		/// <summary>
		/// Gets a reference to the default binder, which implements internal rules for selecting the appropriate members to be called by InvokeMember.
		/// </summary>
		public static Binder DefaultBinder
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the fully qualified name of the Type, including the namespace of the Type but not the assembly.
		/// </summary>
		public abstract string FullName { get; }

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
			get { return this.HasElementTypeImpl(); }
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
			get { return this.IsArrayImpl(); }
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
			get { return this.IsByRefImpl(); }
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
			get { return this.IsContextfulImpl(); }
		}

		/// <summary>
		/// Gets a value indicating whether the current Type represents an enumeration.
		/// </summary>
		public virtual bool IsEnum
		{
			get { return this.IsSubclassOf(typeof(Enum)); }
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
			get { return this.IsPointerImpl(); }
		}

		/// <summary>
		/// Gets a value indicating whether the Type is one of the primitive types.
		/// </summary>
		public bool IsPrimitive
		{
			get { return this.IsPrimitiveImpl(); }
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
			get { return this.IsValueTypeImpl(); }
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
			get { throw new NotSupportedException("The invoked method is not supported in the base class."); }
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
			get
			{
				return this.GetConstructorImpl(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, CallingConventions.Any, Type.EmptyTypes, null);
			}
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
		/// <param name="obj">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
		/// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False. This method also returns False if the object specified by the o parameter is not a Type.</returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Type))
				return false;

			return ((Type)obj).TypeHandle == this.TypeHandle;
		}

		/// <summary>
		/// Determines if the underlying system type of the current Type is the same as the underlying system type of the specified Type.
		/// </summary>
		/// <param name="obj">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
		/// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False.</returns>
		public virtual bool Equals(Type obj)
		{
			return obj.TypeHandle == this.TypeHandle;
		}

		/// <summary>
		/// Gets the number of dimensions in an Array.
		/// </summary>
		/// <returns>An Int32 containing the number of dimensions in the current Type.</returns>
		public virtual int GetArrayRank()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
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
			return this.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any, types, null);
		}

		/// <summary>
		/// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference, to use the DefaultBinder.
		/// </param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the constructor to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a constructor that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. The default binder does not process this parameter.</param>
		/// <returns>A ConstructorInfo object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetConstructor(bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		/// <summary>
		/// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference, to use the DefaultBinder.
		/// </param>
		/// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the constructor to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a constructor that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. The default binder does not process this parameter.</param>
		/// <returns>A ConstructorInfo object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
				throw new ArgumentNullException("types");

			foreach (Type t in types)
			{
				if (t == null)
					throw new ArgumentNullException("types");
			}

			return this.GetConstructorImpl(bindingAttr, binder, callConvention, types, modifiers);
		}

		/// <summary>
		/// When overridden in a derived class, searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference, to use the DefaultBinder.
		/// </param>
		/// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the constructor to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a constructor that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. The default binder does not process this parameter.</param>
		/// <returns>A ConstructorInfo object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
		protected abstract ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

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
			return this.GetField(name, Type.DefaultBindingFlags);
		}

		/// <summary>
		/// Searches for the specified field, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the data field to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>An object representing the field that matches the specified requirements, if found; otherwise, null.</returns>
		public abstract FieldInfo GetField(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Returns all the public fields of the current Type.
		/// </summary>
		/// <returns>
		/// An array of FieldInfo objects representing all the public fields defined for the current Type.
		/// -or- 
		/// An empty array of type FieldInfo, if no public fields are defined for the current Type.
		/// </returns>
		public FieldInfo[] GetFields()
		{
			return this.GetFields(Type.DefaultBindingFlags);
		}

		/// <summary>
		/// When overridden in a derived class, searches for the fields defined for the current Type, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>
		/// An array of FieldInfo objects representing all fields defined for the current Type that match the specified binding constraints.
		/// -or- 
		/// An empty array of type FieldInfo, if no fields are defined for the current Type, or if none of the defined fields match the binding constraints.
		/// </returns>
		public abstract FieldInfo[] GetFields(BindingFlags bindingAttr);

		/// <summary>
		/// Returns an array of Type objects that represent the type arguments of a generic type or the type parameters of a generic type definition.
		/// </summary>
		/// <returns>An array of Type objects that represent the type arguments of a generic type. Returns an empty array if the current type is not a generic type.</returns>
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns an array of Type objects that represent the constraints on the current generic type parameter.
		/// </summary>
		/// <returns>An array of Type objects that represent the constraints on the current generic type parameter.</returns>
		public virtual Type[] GetGenericParameterConstraints()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns a Type object that represents a generic type definition from which the current generic type can be constructed.
		/// </summary>
		/// <returns>A Type object representing a generic type from which the current type can be constructed.</returns>
		public virtual Type[] GetGenericTypeDefinition()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode()
		{
			// TODO
			return base.GetHashCode();
		}

		/// <summary>
		/// Searches for the interface with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
		/// <returns>An object representing the interface with the specified name, implemented or inherited by the current Type, if found; otherwise, null.</returns>
		public Type GetInterface(string name)
		{
			return this.GetInterface(name, false);
		}

		/// <summary>
		/// When overridden in a derived class, searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
		/// </summary>
		/// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
		/// <param name="ignoreCase">
		/// True to ignore the case of that part of name that specifies the simple interface name (the part that specifies the namespace must be correctly cased).
		/// -or-
		/// False to perform a case-sensitive search for all parts of name.
		/// </param>
		/// <returns>An object representing the interface with the specified name, implemented or inherited by the current Type, if found; otherwise, null.</returns>
		public abstract Type GetInterface(string name, bool ignoreCase);

		/// <summary>
		/// When overridden in a derived class, gets all the interfaces implemented or inherited by the current Type.
		/// </summary>
		/// <returns>
		/// An array of Type objects representing all the interfaces implemented or inherited by the current Type.
		/// -or- 
		/// An empty array of type Type, if no interfaces are implemented or inherited by the current Type.
		/// </returns>
		public abstract Type[] GetInterfaces();

		/// <summary>
		/// Searches for the public members with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the public members to get.</param>
		/// <returns>An array of MemberInfo objects representing the public members with the specified name, if found; otherwise, an empty array.</returns>
		public MemberInfo[] GetMember(string name)
		{
			return this.GetMember(name, MemberTypes.All, Type.DefaultBindingFlags);
		}

		/// <summary>
		/// Searches for the specified members, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the members to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>An array of MemberInfo objects representing the public members with the specified name, if found; otherwise, an empty array.</returns>
		public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, MemberTypes.All, bindingAttr);
		}

		/// <summary>
		/// Searches for the specified members of the specified member type, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the members to get.</param>
		/// <param name="type">The value to search for.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>An array of MemberInfo objects representing the public members with the specified name, if found; otherwise, an empty array.</returns>
		public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns all the public members of the current Type.
		/// </summary>
		/// <returns>
		/// An array of MemberInfo objects representing all the public members of the current Type.\
		/// -or- 
		/// An empty array of type MemberInfo, if the current Type does not have public members.
		/// </returns>
		public MemberInfo[] GetMembers()
		{
			return this.GetMembers(Type.DefaultBindingFlags);
		}

		/// <summary>
		/// When overridden in a derived class, searches for the members defined for the current Type, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>
		/// An array of MemberInfo objects representing all members defined for the current Type that match the specified binding constraints.
		/// -or- 
		/// An empty array of type MemberInfo, if no members are defined for the current Type, or if none of the defined members match the binding constraints.
		/// </returns>
		public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the public method with the specified name.
		/// </summary>
		/// <param name="name">The string containing the name of the public method to get.</param>
		/// <returns>An object that represents the public method with the specified name, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			return this.GetMethodImpl(name, DefaultBindingFlags, null, CallingConventions.Any, null, null);
		}

		/// <summary>
		/// Searches for the specified method, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			return this.GetMethodImpl(name, bindingAttr, null, CallingConventions.Any, null, null);
		}

		/// <summary>
		/// Searches for the specified public method whose parameters match the specified argument types.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the method to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// </param>
		/// <returns>An object representing the public method whose parameters match the specified argument types, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name, Type[] types)
		{
			return this.GetMethod(name, Type.DefaultBindingFlags, null, CallingConventions.Any, types, null);
		}

		/// <summary>
		/// Searches for the specified public method whose parameters match the specified argument types and modifiers.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the method to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. To be only used when calling through COM interop, and only parameters that are passed by reference are handled. The default binder does not process this parameter.</param>
		/// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, Type.DefaultBindingFlags, null, types, modifiers);
		}

		/// <summary>
		/// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference to use the DefaultBinder.
		/// </param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the method to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. To be only used when calling through COM interop, and only parameters that are passed by reference are handled. The default binder does not process this parameter.</param>
		/// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		/// <summary>
		/// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference to use the DefaultBinder.
		/// </param>
		/// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and what process cleans up the stack.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the method to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. To be only used when calling through COM interop, and only parameters that are passed by reference are handled. The default binder does not process this parameter.</param>
		/// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			if (types == null)
				throw new ArgumentNullException("types");

			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
					throw new ArgumentNullException("types");
			}

			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		/// <summary>
		/// When overridden in a derived class, searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
		/// </summary>
		/// <param name="name">The string containing the name of the method to get.</param>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference to use the DefaultBinder.
		/// </param>
		/// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and what process cleans up the stack.</param>
		/// <param name="types">
		/// An array of Type objects representing the number, order, and type of the parameters for the method to get.
		/// -or-
		/// An empty array of the type Type (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// -or-
		/// null. If types is null, arguments are not matched.
		/// </param>
		/// <param name="modifiers">An array of ParameterModifier objects representing the attributes associated with the corresponding element in the types array. The default binder does not process this parameter.</param>
		/// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
		protected abstract MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		/// <summary>
		/// Returns all the public methods of the current Type.
		/// </summary>
		/// <returns>
		/// An array of MethodInfo objects representing all the public methods defined for the current Type.
		/// -or- 
		/// An empty array of type MethodInfo, if no public methods are defined for the current Type.
		/// </returns>
		public MethodInfo[] GetMethods()
		{
			return this.GetMethods(Type.DefaultBindingFlags);
		}

		/// <summary>
		/// When overridden in a derived class, searches for the methods defined for the current Type, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// -or-
		/// Zero, to return null.
		/// </param>
		/// <returns>
		/// An array of MethodInfo objects representing all methods defined for the current Type that match the specified binding constraints.
		/// -or- 
		/// An empty array of type MethodInfo, if no methods are defined for the current Type, or if none of the defined methods match the binding constraints.
		/// </returns>
		public abstract MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// !!!!!!!!!!!!!!!!!!!!!!
		// BIG TODO: Propertes!!!
		// !!!!!!!!!!!!!!!!!!!!!!

		/// <summary>
		/// Gets the Type with the specified name, performing a case-sensitive search.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
		/// <returns>The type with the specified name, if found; otherwise, null.</returns>
		public static Type GetType(string typeName)
		{
			return Type.GetTypeImpl(typeName, false, false);
		}

		/// <summary>
		/// Gets the Type with the specified name, performing a case-sensitive search and specifying whether to throw an exception if the type is not found.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
		/// <param name="throwOnError">True to throw an exception if the type cannot be found; False to return null. Specifying False also suppresses some other exception conditions, but not all of them.</param>
		/// <returns>The type with the specified name. If the type is not found, the throwOnError parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of throwOnError.</returns>
		public static Type GetType(string typeName, bool throwOnError)
		{
			return Type.GetTypeImpl(typeName, throwOnError, false);
		}

		/// <summary>
		/// Gets the Type with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
		/// <param name="throwOnError">True to throw an exception if the type cannot be found; False to return null. Specifying False also suppresses some other exception conditions, but not all of them.</param>
		/// <param name="ignoreCase">True to perform a case-insensitive search for typeName, False to perform a case-sensitive search for typeName.</param>
		/// <returns>The type with the specified name. If the type is not found, the throwOnError parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of throwOnError.</returns>
		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			return Type.GetTypeImpl(typeName, throwOnError, ignoreCase);
		}

		/// <summary>
		/// Gets the Type with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
		/// <param name="throwOnError">True to throw an exception if the type cannot be found; False to return null. Specifying False also suppresses some other exception conditions, but not all of them.</param>
		/// <param name="ignoreCase">True to perform a case-insensitive search for typeName, False to perform a case-sensitive search for typeName.</param>
		/// <returns>The type with the specified name. If the type is not found, the throwOnError parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of throwOnError.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetTypeImpl(string typeName, bool throwOnError, bool ignoreCase);

		/// <summary>
		/// Gets the types of the objects in the specified array.
		/// </summary>
		/// <param name="args">An array of objects whose types to determine.</param>
		/// <returns>An array of Type objects representing the types of the corresponding elements in args.</returns>
		public static Type[] GetTypeArray(object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			Type[] argTypes = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				argTypes[i] = args[i].GetType();
			}

			return argTypes;
		}

		/// <summary>
		/// Gets the underlying type code of the specified Type.
		/// </summary>
		/// <param name="type">The type whose underlying type code to get.</param>
		/// <returns>The code of the underlying type, or Empty if type is null.</returns>
		public static TypeCode GetTypeCode(Type type)
		{
			return type.GetTypeCodeImpl();
		}

		/// <summary>
		/// Returns the underlying type code of the specified Type.
		/// </summary>
		/// <returns>The code of the underlying type.</returns>
		protected virtual TypeCode GetTypeCodeImpl()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the type referenced by the specified type handle.
		/// </summary>
		/// <param name="handle">The object that refers to the type.</param>
		/// <returns></returns>
		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			return Type.GetTypeFromHandleImpl(handle);
		}

		/// <summary>
		/// Gets the type referenced by the specified type handle.
		/// </summary>
		/// <param name="handle">The object that refers to the type.</param>
		/// <returns>The type referenced by the specified RuntimeTypeHandle, or null if the Value property of handle is null.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetTypeFromHandleImpl(RuntimeTypeHandle handle);

		/// <summary>
		/// Gets the handle for the Type of a specified object.
		/// </summary>
		/// <param name="o">The object for which to get the type handle.</param>
		/// <returns>The handle for the Type of the specified Object.</returns>
		public static RuntimeTypeHandle GetTypeHandle(object o)
		{
			if (o == null)
				throw new ArgumentNullException("o");

			return o.GetType().TypeHandle;
		}

		/// <summary>
		/// When overridden in a derived class, implements the HasElementType property and determines whether the current Type encompasses or refers to another type; that is, whether the current Type is an array, a pointer, or is passed by reference.
		/// </summary>
		/// <returns>True if the Type is an array, a pointer, or is passed by reference; otherwise, False.</returns>
		protected abstract bool HasElementTypeImpl();

		/// <summary>
		/// Invokes the specified member, using the specified binding constraints and matching the specified argument list.
		/// </summary>
		/// <param name="name">
		/// The string containing the name of the constructor, method, property, or field member to invoke.
		/// -or-
		/// An empty string ("") to invoke the default member.
		/// </param>
		/// <param name="invokeAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on.
		/// The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference , to use the DefaultBinder. Note that explicitly defining a Binder object may be required for successfully invoking method overloads with variable arguments.
		/// </param>
		/// <param name="target">The object on which to invoke the specified member.</param>
		/// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
		/// <returns>An object representing the return value of the invoked member.</returns>
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, null, null);
		}

		/// <summary>
		/// Invokes the specified member, using the specified binding constraints and matching the specified argument list and culture.
		/// </summary>
		/// <param name="name">
		/// The string containing the name of the constructor, method, property, or field member to invoke.
		/// -or-
		/// An empty string ("") to invoke the default member.
		/// </param>
		/// <param name="invokeAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on.
		/// The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference , to use the DefaultBinder. Note that explicitly defining a Binder object may be required for successfully invoking method overloads with variable arguments.
		/// </param>
		/// <param name="target">The object on which to invoke the specified member.</param>
		/// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
		/// <param name="culture">
		/// The CultureInfo object representing the globalization locale to use, which may be necessary for locale-specific conversions, such as converting a numeric String to a Double.
		/// -or-
		/// A null reference to use the current thread's CultureInfo.
		/// </param>
		/// <returns>An object representing the return value of the invoked member.</returns>
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, culture, null);
		}

		/// <summary>
		/// When overridden in a derived class, invokes the specified member, using the specified binding constraints and matching the specified argument list, modifiers and culture.
		/// </summary>
		/// <param name="name">
		/// The string containing the name of the constructor, method, property, or field member to invoke.
		/// -or-
		/// An empty string ("") to invoke the default member.
		/// </param>
		/// <param name="invokeAttr">
		/// A bitmask comprised of one or more BindingFlags that specify how the search is conducted.
		/// The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on.
		/// The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.
		/// </param>
		/// <param name="binder">
		/// An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.
		/// -or-
		/// A null reference , to use the DefaultBinder. Note that explicitly defining a Binder object may be required for successfully invoking method overloads with variable arguments.
		/// </param>
		/// <param name="target">The object on which to invoke the specified member.</param>
		/// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
		/// <param name="modifiers">
		/// An array of ParameterModifier objects representing the attributes associated with the corresponding element in the args array. A parameter's associated attributes are stored in the member's signature. 
		/// The default binder processes this parameter only when calling a COM component.
		/// </param>
		/// <param name="culture">
		/// The CultureInfo object representing the globalization locale to use, which may be necessary for locale-specific conversions, such as converting a numeric String to a Double.
		/// -or-
		/// A null reference to use the current thread's CultureInfo.
		/// </param>
		/// <param name="namedParameters">An array containing the names of the parameters to which the values in the args array are passed.</param>
		/// <returns>An object representing the return value of the invoked member.</returns>
		public abstract object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		/// <summary>
		/// When overridden in a derived class, implements the IsArray property and determines whether the Type is an array.
		/// </summary>
		/// <returns>True if the Type is an array; otherwise, False.</returns>
		protected abstract bool IsArrayImpl();

		/// <summary>
		/// Determines whether an instance of the current Type can be assigned from an instance of the specified Type.
		/// </summary>
		/// <param name="c">The type to compare with the current type.</param>
		/// <returns>
		/// True if c and the current Type represent the same type, or if the current Type is in the inheritance hierarchy of c, or if the current Type is an interface that c implements, or if c is a generic type parameter and the current Type represents one of the constraints of c, or if c represents a value type and the current Type represents Nullable<c>. False if none of these conditions are true, or if c is null.
		/// </returns>
		public virtual bool IsAssignableFrom(Type c)
		{
			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// When overridden in a derived class, implements the IsByRef property and determines whether the Type is passed by reference.
		/// </summary>
		/// <returns>True if the Type is passed by reference; otherwise, False.</returns>
		protected abstract bool IsByRefImpl();

		/// <summary>
		/// Implements the IsContextful property and determines whether the Type can be hosted in a context.
		/// </summary>
		/// <returns>True if the Type can be hosted in a context; otherwise, False.</returns>
		protected virtual bool IsContextfulImpl()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a value that indicates whether the specified value exists in the current enumeration type.
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		/// <returns>True if the specified value is a member of the current enumeration type; otherwise, False.</returns>
		public virtual bool IsEnumDefined(object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			if (!this.IsEnum)
				throw new ArgumentException("The current type is not an enumeration.");

			// TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the specified object is an instance of the current Type.
		/// </summary>
		/// <param name="o">The object to compare with the current type.</param>
		/// <returns>
		/// True if the current Type is in the inheritance hierarchy of the object represented by o, or if the current Type is an interface that o supports.
		/// False if neither of these conditions is the case, or if o is null, or if the current Type is an open generic type (that is, ContainsGenericParameters returns true).
		/// </returns>
		public virtual bool IsInstanceOfType(object o)
		{
			if (o == null || this.ContainsGenericParameters)
				return false;

			var oType = o.GetType();

			for (Type type = oType; type != null; type = type.BaseType)
				if (type == this) return true;

			var oInterfaces = oType.GetInterfaces();

			for (int i = 0; i < oInterfaces.Length; i++)
				if (oInterfaces[i] == this) return true;

			return false;
		}

		/// <summary>
		/// When overridden in a derived class, implements the IsPointer property and determines whether the Type is a pointer.
		/// </summary>
		/// <returns>True if the Type is a pointer; otherwise, False.</returns>
		protected abstract bool IsPointerImpl();

		/// <summary>
		/// When overridden in a derived class, implements the IsPrimitive property and determines whether the Type is one of the primitive types.
		/// </summary>
		/// <returns>True if the Type is one of the primitive types; otherwise, False.</returns>
		protected abstract bool IsPrimitiveImpl();

		/// <summary>
		/// Determines whether the class represented by the current Type derives from the class represented by the specified Type.
		/// </summary>
		/// <param name="c">The type to compare with the current type.</param>
		/// <returns>True if the Type represented by the c parameter and the current Type represent classes, and the class represented by the current Type derives from the class represented by c; otherwise, False. This method also returns False if c and the current Type represent the same class.</returns>
		public virtual bool IsSubclassOf(Type c)
		{
			if (c == null || c == this)
				return false;

			for (Type type = BaseType; type != null; type = type.BaseType)
				if (type == c) return true;

			return false;
		}

		/// <summary>
		/// Implements the IsValueType property and determines whether the Type is a value type; that is, not a class or an interface.
		/// </summary>
		/// <returns>True if the Type is a value type; otherwise, False.</returns>
		protected virtual bool IsValueTypeImpl()
		{
			if (this == typeof(ValueType) || this == typeof(Enum))
				return false;

			return this.IsSubclassOf(typeof(ValueType));
		}

		/// <summary>
		/// Returns a Type object representing a one-dimensional array of the current type, with a lower bound of zero.
		/// </summary>
		/// <returns>A Type object representing a one-dimensional array of the current type, with a lower bound of zero.</returns>
		public virtual Type MakeArrayType()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns a Type object representing an array of the current type, with the specified number of dimensions.
		/// </summary>
		/// <param name="rank">The number of dimensions for the array. This number must be less than or equal to 32.</param>
		/// <returns>An object representing an array of the current type, with the specified number of dimensions.</returns>
		public virtual Type MakeArrayType(int rank)
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns a Type object that represents the current type when passed as a ref parameter.
		/// </summary>
		/// <returns>A Type object that represents the current type when passed as a ref parameter.</returns>
		public virtual Type MakeByRefType()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Substitutes the elements of an array of types for the type parameters of the current generic type definition and returns a Type object representing the resulting constructed type.
		/// </summary>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type.</param>
		/// <returns>A Type representing the constructed type formed by substituting the elements of typeArguments for the type parameters of the current generic type.</returns>
		public virtual Type MakeGenericType(params Type[] typeArguments)
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Returns a Type object that represents a pointer to the current type.
		/// </summary>
		/// <returns>A Type object that represents a pointer to the current type.</returns>
		public virtual Type MakePointerType()
		{
			throw new NotSupportedException("The invoked method is not supported in the base class. Derived classes must provide an implementation.");
		}

		/// <summary>
		/// Gets the Type with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found. The type is loaded for reflection only, not for execution.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the Type to get.</param>
		/// <param name="throwIfNotFound">True to throw a TypeLoadException if the type cannot be found; False to return null if the type cannot be found. Specifying False also suppresses some other exception conditions, but not all of them.</param>
		/// <param name="ignoreCase">True to perform a case-insensitive search for typeName; False to perform a case-sensitive search for typeName.</param>
		/// <returns>The type with the specified name, if found; otherwise, null. If the type is not found, the throwIfNotFound parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of throwIfNotFound.</returns>
		public static Type ReflectionOnlyGetType(string typeName, bool throwIfNotFound, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns a String representing the name of the current Type.
		/// </summary>
		/// <returns>A String representing the name of the current Type.</returns>
		public override string ToString()
		{
			return this.FullName;
		}

		/// <summary>
		/// Indicates whether two Type objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is equal to right; otherwise, False.</returns>
		public static bool operator ==(Type left, Type right)
		{
			return object.ReferenceEquals(left, right);
		}

		/// <summary>
		/// Indicates whether two Type objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>True if left is not equal to right; otherwise, False.</returns>
		public static bool operator !=(Type left, Type right)
		{
			return !(left == right);
		}
	}
}