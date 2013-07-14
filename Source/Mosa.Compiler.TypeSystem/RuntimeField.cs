/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using System;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Base class for the runtime representation of fields.
	/// </summary>
	public abstract class RuntimeField : RuntimeMember
	{
		#region Data members

		/// <summary>
		/// Holds the attributes of the RuntimeField.
		/// </summary>
		private FieldAttributes attributes;

		/// <summary>
		/// Holds the relative virtual address of the field.
		/// </summary>
		private uint rva;

		/// <summary>
		/// Holds the type of the field.
		/// </summary>
		private SigType sigType;

		/// <summary>
		/// Holds the fullname (namespace + declaring type + name)
		/// </summary>
		private string fullname;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeField"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="declaringType">Specifies the type, which contains this field.</param>
		public RuntimeField(ITypeModule module, RuntimeType declaringType, string name) :
			base(module, name, declaringType, Token.Zero)
		{
			this.fullname = (declaringType == null) ? name : String.Format("{0}.{1}", declaringType.FullName, name);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeField"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">Specifies the type, which contains this field.</param>
		public RuntimeField(ITypeModule module, string name, RuntimeType declaringType, Token token) :
			base(module, name, declaringType, token)
		{
			this.fullname = (declaringType == null) ? name : String.Format("{0}.{1}", declaringType.FullName, name);
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		public virtual string FullName
		{
			get { return fullname; }
		}

		/// <summary>
		/// Gets the attributes of the field.
		/// </summary>
		/// <value>The attributes.</value>
		public FieldAttributes Attributes
		{
			get { return attributes; }
			protected set { attributes = value; }
		}

		/// <summary>
		/// Gets the RVA of the initialization data.
		/// </summary>
		/// <value>The RVA of the initialization data.</value>
		public uint RVA
		{
			get { return rva; }
			protected set { rva = value; }
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType SigType
		{
			get { return sigType; }
			protected set { sigType = value; }
		}

		public bool IsLiteralField
		{
			get { return (attributes & FieldAttributes.Literal) == FieldAttributes.Literal; }
		}

		public bool IsStaticField
		{
			get { return (attributes & FieldAttributes.Static) == FieldAttributes.Static; }
		}

		public bool ContainsGenericParameter
		{
			get { return SigType.IsOpenGenericParameter; }
		}

		#endregion Properties

		#region Methods

		#endregion Methods

		#region Object Overrides

		/// <summary>
		/// Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </returns>
		public override string ToString()
		{
			return FullName;
		}

		#endregion Object Overrides
	}
}