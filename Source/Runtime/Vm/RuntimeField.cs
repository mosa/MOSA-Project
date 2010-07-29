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
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// Base class for the runtime representation of fields.
	/// </summary>
	public abstract class RuntimeField : RuntimeMember, IEquatable<RuntimeField>
	{
		#region Data members

		/// <summary>
		/// Holds the attributes of the RuntimeField.
		/// </summary>
		private FieldAttributes attributes;

		/// <summary>
		/// Holds the relative virtual address of the field.
		/// </summary>
		private IntPtr rva;

		/// <summary>
		/// Holds the type of the field.
		/// </summary>
		private FieldSignature signature;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeField"/>.
		/// </summary>
		/// <param name="module">The module the field belongs to.</param>
		/// <param name="declaringType">Specifies the type, which contains this field.</param>
		public RuntimeField(IMetadataModule module, RuntimeType declaringType) :
			base(0, module, declaringType, null)
		{
		}
		
		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the attributes of the field.
		/// </summary>
		/// <value>The attributes.</value>
		public FieldAttributes Attributes
		{
			get { return this.attributes; }
			protected set { this.attributes = value; }
		}

		/// <summary>
		/// Gets the RVA of the initialization data.
		/// </summary>
		/// <value>The RVA of the initialization data.</value>
		public IntPtr RVA
		{
			get { return rva; }
			protected set { this.rva = value; }
		}
		
		public FieldSignature Signature
		{
			get
			{
				if (this.signature == null)
					this.signature = this.GetSignature();
				
				return this.signature;
			}
			
			protected set
			{
				this.signature = value;
			}
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType SignatureType
		{
			get
			{
				return this.Signature.Type;
			}
		}

		public RuntimeType Type
		{
			get
			{
				// HACK: Generic fields -- is this right?
				return RuntimeBase.Instance.TypeLoader.ResolveSignatureType(DefaultSignatureContext.Instance, this.Module, this.SignatureType);
			}
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Gets the type of the field.
		/// </summary>
		/// <returns>The type of the field.</returns>
		protected abstract FieldSignature GetSignature();

		#endregion // Methods

		#region Object Overrides

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			string name;
			RuntimeType declaringType = this.DeclaringType;
			if (declaringType != null)
			{
				string declaringTypeSymbolName = declaringType.ToString();
				name = String.Format("{0}.{1}", declaringTypeSymbolName, this.Name);
			}
			else
			{
				name = this.Name;
			}

			return name;
		}

		#endregion // Object Overrides

		#region IEquatable<RuntimeField> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public virtual bool Equals(RuntimeField other)
		{
			return (Module == other.Module && this.attributes == other.attributes);
		}

		#endregion // IEquatable<RuntimeField> Members
	}
}
