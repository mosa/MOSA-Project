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
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using System.Diagnostics;

namespace Mosa.Runtime.TypeLoader
{
	/// <summary>
	/// Base class for the runtime representation of methods.
	/// </summary>
	public abstract class RuntimeMethod : RuntimeMember, IEquatable<RuntimeMethod>
	{
		#region Data members

		/// <summary>
		/// Holds the generic arguments of the method.
		/// </summary>
		private GenericArgument[] genericArguments;

		/// <summary>
		/// The implementation attributes of the method.
		/// </summary>
		private MethodImplAttributes implFlags;

		/// <summary>
		/// Generic attributes of the method.
		/// </summary>
		private MethodAttributes attributes;

		/// <summary>
		/// Holds the signature of the method.
		/// </summary>
		private MethodSignature signature;

		/// <summary>
		/// Holds the list of parameters of the method.
		/// </summary>
		private IList<RuntimeParameter> parameters;

		/// <summary>
		/// Holds the rva of the MSIL of the method.
		/// </summary>
		private ulong rva;

		//private ExceptionClauseHeader exceptionClauseHeader = new ExceptionClauseHeader();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeMethod"/> class.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">The type, which declared this method.</param>
		public RuntimeMethod(int token, RuntimeType declaringType) :
			base(token, declaringType, null)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the method attributes.
		/// </summary>
		/// <value>The attributes.</value>
		public MethodAttributes Attributes
		{
			get { return attributes; }
			protected set { attributes = value; }
		}

		/// <summary>
		/// Determines if the method is a generic method.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is generic; otherwise, <c>false</c>.
		/// </value>
		public bool IsGeneric
		{
			get { return this.genericArguments != null; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is abstract.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is abstract; otherwise, <c>false</c>.
		/// </value>
		public bool IsAbstract
		{
			get { return (attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsNative
		{
			get { return Rva == 0; }
		}

		/// <summary>
		/// Gets the method implementation attributes.
		/// </summary>
		/// <value>The impl attributes.</value>
		public MethodImplAttributes ImplAttributes
		{
			get { return implFlags; }
			protected set { implFlags = value; }
		}

		/// <summary>
		/// Returns the parameter definitions of this method.
		/// </summary>
		/// <value>The parameters.</value>
		public IList<RuntimeParameter> Parameters
		{
			get { return parameters; }
			protected set { parameters = value; }
		}

		/// <summary>
		/// Retrieves the signature of the method.
		/// </summary>
		/// <value>The signature.</value>
		public MethodSignature Signature
		{
			get { return signature; }
			protected set { signature = value; }
		}

		/// <summary>
		/// Holds the RVA of the method in the binary.
		/// </summary>
		/// <value>The rva.</value>
		public ulong Rva
		{
			get { return this.rva; }
			protected set { this.rva = value; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Sets generic parameters on this method.
		/// </summary>
		/// <param name="gprs">A list of generic parameters to set on the method.</param>
		public void SetGenericParameter(List<GenericParamRow> gprs)
		{
			// TODO: Implement this method
			this.genericArguments = new GenericArgument[gprs.Count];
		}

		#endregion // Methods

		#region IEquatable<RuntimeMethod> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public virtual bool Equals(RuntimeMethod other)
		{
			//TODO
			//return (base.Equals(other) && MetadataModule == other.MetadataModule);
			return (base.Equals(other));
		}

		#endregion // IEquatable<RuntimeMethod> Members

		#region Object Overrides

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			//TODO: The result of this method should be cached to improve performance
			StringBuilder result = new StringBuilder();

			result.Append(this.DeclaringType.ToString());
			result.Append('.');
			result.Append(this.Name);
			result.Append('(');

			if (0 != this.Parameters.Count)
			{
				MethodSignature sig = this.Signature;
				int i = 0;
				foreach (RuntimeParameter p in this.Parameters)
				{
					result.AppendFormat("{0} {1},", sig.Parameters[i++].Type, p.Name);
				}
				result.Remove(result.Length - 1, 1);
			}

			result.Append(')');

			return result.ToString();
		}

		#endregion // Object Overrides


	}
}
