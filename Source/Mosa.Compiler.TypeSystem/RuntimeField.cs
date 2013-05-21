/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

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
		private FieldSignature signature;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeField"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="declaringType">Specifies the type, which contains this field.</param>
		public RuntimeField(ITypeModule module, RuntimeType declaringType) :
			base(module, Token.Zero, declaringType)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeField"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">The token.</param>
		/// <param name="declaringType">Specifies the type, which contains this field.</param>
		public RuntimeField(ITypeModule module, Token token, RuntimeType declaringType) :
			base(module, token, declaringType)
		{
		}

		#endregion Construction

		#region Properties

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

		public FieldSignature Signature
		{
			get { return signature; }
			protected set { signature = value; }
		}

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public SigType SignatureType
		{
			get { return this.Signature.Type; }
			internal set { this.Signature.Type = value; }
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
			get { return Signature.Type.IsOpenGenericParameter; }
		}

		#endregion Properties

		#region Methods

		#endregion Methods

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
			var declaringType = this.DeclaringType;
			if (declaringType != null)
			{
				var declaringTypeSymbolName = declaringType.ToString();
				name = String.Format("{0}.{1}", declaringTypeSymbolName, this.Name);
			}
			else
			{
				name = this.Name;
			}

			return name;
		}

		#endregion Object Overrides
	}
}