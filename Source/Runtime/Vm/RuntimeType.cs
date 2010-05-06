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

        private IList<RuntimeMethod> methodTable;

        /// <summary>
        /// Holds the (cached) namespace of the type.
        /// </summary>
        private string @namespace;

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
        private IEnumerable<RuntimeMethod> methods;

        /// <summary>
        /// Holds the fields of this type.
        /// </summary>
        private IList<RuntimeField> fields;

        private bool isCompiled;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeType"/> class.
        /// </summary>
        /// <param name="token">The token of the type.</param>
        /// <param name="module">The module.</param>
        public RuntimeType(int token, IMetadataModule module) :
            base(token, module, null, null)
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
                if (this.baseType == null)
                {
                    this.baseType = GetBaseType();
                }

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

        public bool IsValueType
        {
            get
            {
                RuntimeType valueType = RuntimeBase.Instance.TypeLoader.GetType(@"System.ValueType");
                return this.IsSubclassOf(valueType);
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
        /// Returns the methods of the type.
        /// </summary>
        /// <value>The methods.</value>
        public IEnumerable<RuntimeMethod> Methods
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
                if (this.@namespace == null)
                {
                    this.@namespace = GetNamespace();
                    Debug.Assert(this.@namespace != null, @"GetNamespace() failed");
                }

                return this.@namespace;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(@"value");
                if (this.@namespace != null)
                    throw new InvalidOperationException();

                this.@namespace = value;
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

        // FIXME: This list of methods is pretty specific to type loading/compilation. I don't think
        // we need to keep it around as long as we do it right now.
        public IList<RuntimeMethod> MethodTable
        {
            get { return this.methodTable; }
            set
            {
                Debug.Assert(value != null, @"Assigning null method table.");
                this.methodTable = value;
            }
        }

        /// <summary>
        /// Gets the packing of type fields.
        /// </summary>
        /// <value>The packing of type fields.</value>
        public int Pack
        {
            get { return this.packing; }
            protected set
            {
                this.packing = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the type.
        /// </summary>
        /// <value>The size of the type.</value>
        public int Size
        {
            get { return this.nativeSize; }
            set { this.nativeSize = value; }
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
                MethodAttributes attrs = MethodAttributes.SpecialName|MethodAttributes.RTSpecialName|MethodAttributes.Static;
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
                RuntimeType delegateType = RuntimeBase.Instance.TypeLoader.GetType(@"System.Delegate, mscorlib");
                return this.IsSubclassOf(delegateType);
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
    }
}
