/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using System.Diagnostics;

namespace Mosa.Runtime.TypeLoader
{
	/// <summary>
	/// Internal runtime representation of a type.
	/// </summary>
	public abstract class RuntimeType : RuntimeMember, IEquatable<RuntimeType>
	{
		#region Data members

		/// <summary>
		/// Holds the generic arguments of the type.
		/// </summary>
		private GenericArgument[] arguments;

		/// <summary>
		/// Holds the base type of this type.
		/// </summary>
		private RuntimeType baseType;

		/// <summary>
		/// Holds the type flag.
		/// </summary>
		private TypeAttributes flags;

		/// <summary>
		/// Holds the (cached) namespace of the type.
		/// </summary>
		private string nameSpace;

		/// <summary>
		/// Holds the calculated native size of the type.
		/// </summary>
		private int nativeSize;

		/// <summary>
		/// Holds the field packing.
		/// </summary>
		private int packing;

		/// <summary>
		/// Methods of the type.
		/// </summary>
		private IList<RuntimeMethod> methods;

		/// <summary>
		/// Holds the fields of this type.
		/// </summary>
		private IList<RuntimeField> fields;

		/// <summary>
		/// Holds the interfaces of this type.
		/// </summary>
		private IList<RuntimeType> interfaces;

		/// <summary>
		/// 
		/// </summary>
		private IList<RuntimeType> nestedTypes;

		private bool isCompiled;

		private bool isValueType;
		private bool isDelegate;
		private bool isEnum;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeType"/> class.
		/// </summary>
		/// <param name="token">The token of the type.</param>
		public RuntimeType(int token) :
			base(token, null, null)
		{
			//TODO
			//RuntimeType valueType = moduleTypeSystem.TypeSystem.GetType(@"System.ValueType");
			//isValueType = this.IsSubclassOf(valueType);
			isValueType = false;

			//TODO
			//RuntimeType delegateType = moduleTypeSystem.TypeSystem.GetType(@"System.Delegate, mscorlib");
			//isDelegate = IsSubclassOf(delegateType);
			isDelegate = false;

			//TODO
			//RuntimeType enumType = moduleTypeSystem.TypeSystem.GetType(@"System.Enum");
			//isEnum = ReferenceEquals(BaseType, enumType);
			isEnum = false;

			//TODO
			//interfaces = null;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <value>The attributes.</value>
		public TypeAttributes Attributes
		{
			get { return flags; }
			protected set { flags = value; }
		}

		/// <summary>
		/// Retrieves the base class of the represented type.
		/// </summary>
		/// <value>The extends.</value>
		public RuntimeType BaseType
		{
			get { return baseType; }
			protected set { baseType = value; }
		}

		/// <summary>
		/// Determines if the type has generic arguments.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is generic; otherwise, <c>false</c>.
		/// </value>
		public bool IsGeneric
		{
			get { return (arguments != null && arguments.Length != 0); }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is value type.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is value type; otherwise, <c>false</c>.
		/// </value>
		public bool IsValueType
		{
			get { return isValueType; }
		}

		/// <summary>
		/// Gets a value indicating whether type is a module.
		/// </summary>
		/// <value><c>true</c> if this instance is module; otherwise, <c>false</c>.</value>
		public bool IsModule
		{
			get { return (Name.Equals("<Module>") && Namespace.Length == 0); }
		}

		/// <summary>
		/// Returns the fields of the type.
		/// </summary>
		/// <value>The fields.</value>
		public IList<RuntimeField> Fields
		{
			get { return fields; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (fields != null)
					throw new InvalidOperationException();

				fields = value;
			}
		}

		/// <summary>
		/// Returns the interfaces implemented by this type.
		/// </summary>
		/// <value>A list of interfaces.</value>
		public IList<RuntimeType> Interfaces
		{
			get { return interfaces; }
			protected set { interfaces = value; }
		}

		/// <summary>
		/// Returns the methods of the type.
		/// </summary>
		/// <value>The methods.</value>
		public IList<RuntimeMethod> Methods
		{
			get { return methods; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (methods != null)
					throw new InvalidOperationException();

				methods = value;
			}
		}

		/// <summary>
		/// Retrieves the namespace of the represented type.
		/// </summary>
		/// <value>The namespace.</value>
		public string Namespace
		{
			get { return nameSpace; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (nameSpace != null)
					throw new InvalidOperationException();

				nameSpace = value;
			}
		}

		/// <summary>
		/// Gets the full name of the type.
		/// </summary>
		/// <value>The full name.</value>
		public string FullName
		{
			get
			{
				string ns = Namespace;
				string name = Name;
				if (ns == null)
					return name;

				return ns + "." + name;
			}
		}

		/// <summary>
		/// Gets the packing of type fields.
		/// </summary>
		/// <value>The packing of type fields.</value>
		public int Pack
		{
			get { return packing; }
			protected set { packing = value; }
		}

		/// <summary>
		/// Gets or sets the size of the type.
		/// </summary>
		/// <value>The size of the type.</value>
		public int Size
		{
			get { return nativeSize; }
			protected set { nativeSize = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public IList<RuntimeType> NestedTypes
		{
			get { return this.nestedTypes; }
			protected set { this.nestedTypes = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool HasNestedTypes
		{
			get { return this.NestedTypes != null && this.NestedTypes.Count > 0; }
		}

		/// <summary>
		/// Gets the type initializer.
		/// </summary>
		/// <value>The type initializer.</value>
		public RuntimeMethod TypeInitializer
		{
			get
			{
				RuntimeMethod result = null;
				MethodAttributes attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
				foreach (RuntimeMethod method in this.Methods)
				{
					if ((method.Attributes & attrs) == attrs && method.Name == ".cctor")
					{
						Debug.Assert(method.Parameters.Count == 0, @"Static initializer takes arguments??");
						Debug.Assert(method.Signature.ReturnType == null, @"Static initializer having a result??");
						result = method;
						break;
					}
				}
				return result;
			}
		}

		public bool IsExplicitLayoutRequestedByType
		{
			get { return (flags & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Determines whether instances of <paramref name="type"/> can be assigned to variables of this type.
		/// </summary>
		/// <param name="type">The type to check assignment for.</param>
		/// <returns>
		/// 	<c>true</c> if <paramref name="type "/> is assignable to this type; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAssignableFrom(RuntimeType type)
		{
			if (type == null)
				throw new ArgumentNullException(@"type");

			// FIXME: We're not checking interfaces yet
			// FIXME: Only works for classes
			Debug.Assert((flags & TypeAttributes.Class) == TypeAttributes.Class, @"Only works for classes!");

			return (Equals(type) || type.IsSubclassOf(this));
		}

		/// <summary>
		/// Determines whether the class represented by this RuntimeType is a subclass of the type represented by c.
		/// </summary>
		/// <param name="c">The type to compare with the current type.</param>
		/// <returns>
		/// <c>true</c> if the Type represented by the c parameter and the current Type represent classes, and the 
		/// class represented by the current Type derives From the class represented by c; otherwise, <c>false</c>. 
		/// This method also returns <c>false</c> if c and the current Type represent the same class.
		/// </returns>
		public bool IsSubclassOf(RuntimeType c)
		{
			if (c == null)
				throw new ArgumentNullException(@"c");

			RuntimeType baseType = BaseType;
			while (baseType != null)
			{
				if (baseType.Equals(c))
					return true;

				RuntimeType nextBaseType = baseType.BaseType;
				if (baseType.Equals(nextBaseType))
					break;

				baseType = nextBaseType;
			}

			return false;
		}

		/// <summary>
		/// Sets generic parameters on this method.
		/// </summary>
		/// <param name="gprs">A list of generic parameters to set on the method.</param>
		public void SetGenericParameter(List<GenericParamRow> gprs)
		{
			// TODO: Implement this method
			arguments = new GenericArgument[gprs.Count];
		}

		#endregion // Methods

		#region IEquatable<RuntimeType> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public virtual bool Equals(RuntimeType other)
		{
			return (flags == other.flags && nativeSize == other.nativeSize && packing == other.packing);
		}

		#endregion // IEquatable<RuntimeType> Members

		#region Object Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.FullName;
		}

		#endregion // Object Overrides

		public bool IsDelegate
		{
			get { return isDelegate; }
		}

		public bool IsEnum
		{
			get { return isEnum; }
		}

		public bool IsInterface
		{
			get { return (Attributes & TypeAttributes.Interface) == TypeAttributes.Interface; }
		}

		public bool IsCompiled
		{
			get { return isCompiled; }
			set { isCompiled = value; }
		}


		public RuntimeMethod FindMethod(string name)
		{
			foreach (RuntimeMethod method in Methods)
			{
				if (name == method.Name)
				{
					return method;
				}
			}

			throw new MissingMethodException(Name, name);
		}

		public virtual bool ContainsOpenGenericParameters
		{
			get { return false; }
		}


	}
}
