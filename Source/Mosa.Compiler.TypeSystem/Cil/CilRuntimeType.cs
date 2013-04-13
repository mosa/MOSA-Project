/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Diagnostics;

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.TypeSystem.Cil
{
	/// <summary>
	/// Runtime representation of a CIL type.
	/// </summary>
	internal sealed class CilRuntimeType : RuntimeType
	{
		#region Data Members

		/// <summary>
		/// Holds the token of the base type of this type.
		/// </summary>
		private readonly Token baseTypeToken;

		/// <summary>
		/// Holds the enclosing type, if any.
		/// </summary>
		private readonly RuntimeType enclosingType;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeType"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="typeNamespace">The type namespace.</param>
		/// <param name="packing">The packing.</param>
		/// <param name="size">The size.</param>
		/// <param name="token">The token.</param>
		/// <param name="baseType">Type of the base.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="attributes">The attributes.</param>
		/// <param name="baseToken">The base token.</param>
		public CilRuntimeType(ITypeModule module, string name, string typeNamespace, int packing, int size, Token token, RuntimeType baseType, RuntimeType enclosingType, TypeAttributes attributes, Token baseToken) :
			base(module, token, baseType)
		{
			this.baseTypeToken = baseToken;
			this.enclosingType = enclosingType;

			base.Attributes = attributes;
			base.Pack = packing;
			base.LayoutSize = size;
			base.Name = name;
			base.Namespace = typeNamespace;

			if (IsNested)
			{
				Debug.Assert(enclosingType != null);
				this.Namespace = enclosingType.Namespace + "." + enclosingType.Name;
			}
		}

		#endregion Construction

		#region Methods

		public bool IsNested
		{
			get
			{
				if ((Attributes & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic) return true;
				if ((Attributes & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate) return true;
				if ((Attributes & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily) return true;
				return false;
			}
		}

		#endregion Methods
	}
}