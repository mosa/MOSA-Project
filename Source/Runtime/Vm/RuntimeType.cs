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

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using System.Diagnostics;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// Internal runtime representation of a type.
	/// </summary>
	public abstract class RuntimeType : RuntimeMember, IEquatable<RuntimeType>, ISignatureContext
	{
		#region Data members

		/// <summary>
		/// An array used to describe types, which do not implement any interface.
		/// </summary>
		public static readonly RuntimeType[] NoInterfaces = new RuntimeType[0];

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

		private bool isCompiled;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeType"/> class.
		/// </summary>
		/// <param name="moduleTypeSystem"></param>
		/// <param name="token">The token of the type.</param>
		public RuntimeType(IModuleTypeSystem moduleTypeSystem, int token) :
			base(moduleTypeSystem, token, null, null)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <value>The attributes.</value>
		public TypeAttributes Attributes
		{
			get { return this.flags; }
			protected set { this.flags = value; }
		}

		/// <summary>
		/// Retrieves the base class of the represented type.
		/// </summary>
		/// <value>The extends.</value>
		public RuntimeType BaseType
		{
			get
			{
				this.AssertBaseTypeIsLoaded();
				return this.baseType;
			}
		}

		/// <summary>
		/// Determines if the type has generic arguments.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is generic; otherwise, <c>false</c>.
		/// </value>
		public bool IsGeneric
		{
			get { return (this.arguments != null && this.arguments.Length != 0); }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is value type.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is value type; otherwise, <c>false</c>.
		/// </value>
		public bool IsValueType
		{
			get
			{
				this.AssertBaseTypeIsLoaded();

				RuntimeType valueType = moduleTypeSystem.TypeSystem.GetType(@"System.ValueType");
				return this.IsSubclassOf(valueType);
			}
		}

		/// <summary>
		/// Gets a value indicating whether type is a module.
		/// </summary>
		/// <value><c>true</c> if this instance is module; otherwise, <c>false</c>.</value>
		public bool IsModule
		{
			get
			{
				return (Name.Equals("<Module>") && Namespace.Length == 0);
			}
		}

		/// <summary>
		/// Returns the fields of the type.
		/// </summary>
		/// <value>The fields.</value>
		public IList<RuntimeField> Fields
		{
			get { return this.fields; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (this.fields != null)
					throw new InvalidOperationException();

				this.fields = value;
			}
		}

		/// <summary>
		/// Returns the interfaces implemented by this type.
		/// </summary>
		/// <value>A list of interfaces.</value>
		public IList<RuntimeType> Interfaces
		{
			get
			{
				if (this.interfaces == null)
				{
					this.interfaces = this.LoadInterfaces();
				}

				return this.interfaces;
			}
		}

		/// <summary>
		/// Returns the methods of the type.
		/// </summary>
		/// <value>The methods.</value>
		public IList<RuntimeMethod> Methods
		{
			get { return this.methods; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (this.methods != null)
					throw new InvalidOperationException();

				this.methods = value;
			}
		}

		/// <summary>
		/// Retrieves the namespace of the represented type.
		/// </summary>
		/// <value>The namespace.</value>
		public string Namespace
		{
			get
			{
				if (this.nameSpace == null)
				{
					this.nameSpace = GetNamespace();
					Debug.Assert(this.nameSpace != null, @"GetNamespace() failed");
				}

				return this.nameSpace;
			}

			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (this.nameSpace != null)
					throw new InvalidOperationException();

				this.nameSpace = value;
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
				string ns = this.Namespace, name = this.Name;
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
			get { return this.packing; }
			protected set { this.packing = value; }
		}

		/// <summary>
		/// Gets or sets the size of the type.
		/// </summary>
		/// <value>The size of the type.</value>
		public int Size							// FIXME: should be determined by TypeLayoutStage
		{
			get { return this.nativeSize; }
			set { this.nativeSize = value; }	// FIXME: should be protected
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
			get
			{
				return (flags & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
			}
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Gets the base type.
		/// </summary>
		/// <returns>The base type.</returns>
		protected abstract RuntimeType GetBaseType();

		/// <summary>
		/// Called to retrieve the namespace of the type.
		/// </summary>
		/// <returns>The namespace of the type.</returns>
		protected abstract string GetNamespace();

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
			Debug.Assert((this.flags & TypeAttributes.Class) == TypeAttributes.Class, @"Only works for classes!");

			return (this.Equals(type) == true || type.IsSubclassOf(this) == true);
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

			RuntimeType baseType = this.BaseType;
			while (baseType != null)
			{
				if (baseType.Equals(c) == true)
					return true;

				RuntimeType nextBaseType = baseType.BaseType;
				if (baseType.Equals(nextBaseType) == true)
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
			this.arguments = new GenericArgument[gprs.Count];
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
			return (this.flags == other.flags && this.nativeSize == other.nativeSize && this.packing == other.packing);
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

		public SigType GetGenericMethodArgument(int index)
		{
			return DefaultSignatureContext.Instance.GetGenericMethodArgument(index);
		}

		public virtual SigType GetGenericTypeArgument(int index)
		{
			return DefaultSignatureContext.Instance.GetGenericTypeArgument(index);
		}

		public bool IsDelegate
		{
			get
			{
				this.AssertBaseTypeIsLoaded();

				RuntimeType delegateType = moduleTypeSystem.TypeSystem.GetType(@"System.Delegate, mscorlib");
				return this.IsSubclassOf(delegateType);
			}
		}

		public bool IsEnum
		{
			get
			{
				this.AssertBaseTypeIsLoaded();

				RuntimeType enumType = moduleTypeSystem.TypeSystem.GetType(@"System.Enum");
				return ReferenceEquals(this.BaseType, enumType);
			}
		}

		public bool IsInterface
		{
			get
			{
				return (this.Attributes & TypeAttributes.Interface) == TypeAttributes.Interface;
			}
		}

		public bool IsCompiled
		{
			get
			{
				return this.isCompiled;
			}

			set
			{
				this.isCompiled = value;
			}
		}


		private void AssertBaseTypeIsLoaded()
		{
			if (this.baseType == null)
			{
				this.baseType = GetBaseType();
			}
		}

		public RuntimeMethod FindMethod(string name)
		{
			foreach (RuntimeMethod method in this.Methods)
			{
				if (name == method.Name)
				{
					return method;
				}
			}

			throw new MissingMethodException(this.Name, name);
		}

		/// <summary>
		/// Loads the interfaces implemented by a type.
		/// </summary>
		protected abstract IList<RuntimeType> LoadInterfaces();
	}
}
