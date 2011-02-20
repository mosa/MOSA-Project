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
using System.Diagnostics;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeSystem.Cil
{
	/// <summary>
	/// Runtime representation of a CIL type.
	/// </summary>
	sealed class CilRuntimeType : RuntimeType
	{
		#region Data Members

		/// <summary>
		/// Holds the token of the base type of this type.
		/// </summary>
		private readonly TokenTypes baseTypeToken;

		/// <summary>
		/// 
		/// </summary>
		private readonly RuntimeType EnclosingType;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeType"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="typenamespace">The typenamespace.</param>
		/// <param name="packing">The packing.</param>
		/// <param name="size">The size.</param>
		/// <param name="token">The token.</param>
		/// <param name="baseType">Type of the base.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="typeDefRow">The type def row.</param>
		public CilRuntimeType(ITypeModule module, string name, string typenamespace, int packing, int size, TokenTypes token, RuntimeType baseType, RuntimeType enclosingType, TypeDefRow typeDefRow) :
			base(module, token, baseType)
		{
			this.baseTypeToken = typeDefRow.Extends;
			base.Attributes = typeDefRow.Flags;
			base.Pack = packing;
			base.LayoutSize = size;
			base.Name = name;
			base.Namespace = typenamespace;
			this.EnclosingType = enclosingType;

			if (IsNested)
			{
				Debug.Assert(enclosingType != null);
				this.Namespace = enclosingType.Namespace + "." + enclosingType.Name;
			}
		}

		#endregion // Construction

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

		#endregion // Methods
	}
}
