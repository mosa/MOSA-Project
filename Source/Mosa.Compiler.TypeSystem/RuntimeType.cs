/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Internal runtime representation of a type.
	/// </summary>
	public abstract class RuntimeType : RuntimeMember
	{
		#region Data members

		/// <summary>
		/// Holds the base type of this type.
		/// </summary>
		private readonly RuntimeType baseType;

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
		private int layoutSize;

		/// <summary>
		/// Holds the field packing.
		/// </summary>
		private int packing;

		/// <summary>
		/// Methods of the type.
		/// </summary>
		private readonly IList<RuntimeMethod> methods;

		/// <summary>
		/// Holds the fields of this type.
		/// </summary>
		private readonly IList<RuntimeField> fields;

		/// <summary>
		/// Holds the interfaces of this type.
		/// </summary>
		private readonly IList<RuntimeType> interfaces;

		/// <summary>
		///
		/// </summary>
		private readonly bool isValueType;

		/// <summary>
		///
		/// </summary>
		private readonly bool isDelegate;

		/// <summary>
		///
		/// </summary>
		private readonly bool isEnum;

		/// <summary>
		///
		/// </summary>
		private IList<GenericParameter> genericParameters;

		/// <summary>
		/// Holds the fullname (namespace + declaring type + name)
		/// </summary>
		private string fullname;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeType"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">The token of the type.</param>
		/// <param name="baseType">Type of the base.</param>
		public RuntimeType(ITypeModule module, Token token, string name, RuntimeType baseType, string nameSpace) :
			base(module, name, null, token)
		{
			this.baseType = baseType;
			this.nameSpace = nameSpace;

			if (baseType == null)
			{
				this.isValueType = false;
				this.isDelegate = false;
				this.isEnum = false;
			}
			else
			{
				if (baseType.isValueType)
					this.isValueType = true;
				else
					if (baseType.FullName == "System.ValueType")
						this.isValueType = true;

				if (baseType.isDelegate)
					this.isDelegate = true;
				else
					if (baseType.FullName == "System.Delegate")
						this.isDelegate = true;

				if (baseType.isEnum)
					this.isEnum = true;
				else
					if (baseType.FullName == "System.Enum")
						this.isEnum = true;
			}

			this.fields = new List<RuntimeField>();
			this.methods = new List<RuntimeMethod>();
			this.interfaces = new List<RuntimeType>();
			this.genericParameters = new List<GenericParameter>();
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		public string FullName
		{
			get
			{
				if (fullname == null)
				{
					fullname = (DeclaringType == null) ? Name : String.Format("{0}.{1}", DeclaringType.FullName, Name);

					if (nameSpace != null)
					{
						fullname = nameSpace + "." + fullname;
					}
				}

				return fullname;
			}
		}


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
		}

		/// <summary>
		/// Determines if the type has generic arguments.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is generic; otherwise, <c>false</c>.
		/// </value>
		public bool IsGeneric
		{
			get { return genericParameters.Count != 0; }
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
		}

		/// <summary>
		/// Returns the interfaces implemented by this type.
		/// </summary>
		/// <value>A list of interfaces.</value>
		public IList<RuntimeType> Interfaces
		{
			get { return interfaces; }
		}

		/// <summary>
		/// Returns the methods of the type.
		/// </summary>
		/// <value>The methods.</value>
		public IList<RuntimeMethod> Methods
		{
			get { return methods; }
		}

		/// <summary>
		/// Retrieves the namespace of the represented type.
		/// </summary>
		/// <value>The namespace.</value>
		public string Namespace
		{
			get { return nameSpace; }
			protected set { nameSpace = value; }
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
		public int LayoutSize
		{
			get { return layoutSize; }
			protected set { layoutSize = value; }
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
				var attrs = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Static;
				foreach (var method in this.Methods)
				{
					if ((method.Attributes & attrs) == attrs && method.Name == ".cctor")
					{
						Debug.Assert(method.Parameters.Count == 0, @"Static initializer takes arguments??");
						Debug.Assert(method.ReturnType == null, @"Static initializer having a result??");
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

		/// <summary>
		/// Returns the interfaces implemented by this type.
		/// </summary>
		/// <value>A list of interfaces.</value>
		public IList<GenericParameter> GenericParameters
		{
			get { return genericParameters; }
			protected set { genericParameters = value; }
		}

		#endregion Properties

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

			var baseType = BaseType;
			while (baseType != null)
			{
				if (baseType.Equals(c))
					return true;

				var nextBaseType = baseType.BaseType;
				if (baseType.Equals(nextBaseType))
					break;

				baseType = nextBaseType;
			}

			return false;
		}

		#endregion Methods

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

		#endregion Object Overrides

		public RuntimeMethod FindMethod(string name)
		{
			foreach (var method in Methods)
			{
				if (name == method.Name)
				{
					return method;
				}
			}

			return null;

			//throw new MissingMethodException(Name, name);
		}

		public virtual bool ContainsOpenGenericParameters
		{
			get { return genericParameters.Count != 0; }
		}

		public bool ImplementsInterface(RuntimeType interfaceType)
		{
			Debug.Assert(interfaceType.IsInterface);

			if (interfaces.Contains(interfaceType))
				return true;
			else if (baseType != null)
				return baseType.ImplementsInterface(interfaceType);
			else
				return false;
		}
	}
}