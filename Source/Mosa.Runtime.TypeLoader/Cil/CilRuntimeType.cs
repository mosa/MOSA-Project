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

using Mosa.Runtime.TypeLoader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.TypeLoader.Cil
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
		private TokenTypes baseTypeToken;

		/// <summary>
		/// The name index of the defined type.
		/// </summary>
		private TokenTypes nameIdx;

		/// <summary>
		/// The namespace index of the defined type.
		/// </summary>
		private TokenTypes namespaceIdx;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeType"/> class.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="token">The token.</param>
		/// <param name="typeDefRow">The type def row.</param>
		/// <param name="packing">The packing.</param>
		/// <param name="size">The size.</param>
		public CilRuntimeType(IMetadataProvider metadataProvider, TokenTypes token, TypeDefRow typeDefRow, int packing, int size, RuntimeType baseType) :
			base((int)token)
		{
			this.baseTypeToken = typeDefRow.Extends;
			this.nameIdx = typeDefRow.TypeNameIdx;
			this.namespaceIdx = typeDefRow.TypeNamespaceIdx;
			base.Attributes = typeDefRow.Flags;
			base.Pack = packing;
			base.Size = size;
			base.BaseType = baseType;

			this.Name = metadataProvider.ReadString(this.nameIdx);
			Debug.Assert(this.Name != null, @"Failed to retrieve CilRuntimeMethod name.");

			this.Fields = new List<RuntimeField>();
			this.Methods = new List<RuntimeMethod>();
			this.Interfaces = new List<RuntimeType>();

			this.Namespace = GetNamespace(metadataProvider);
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(RuntimeType other)
		{
			CilRuntimeType crt = other as CilRuntimeType;
			return (crt != null &&
				//this.moduleTypeSystem == crt.moduleTypeSystem &&
				this.nameIdx == crt.nameIdx &&
				this.namespaceIdx == crt.namespaceIdx &&
				this.baseTypeToken == crt.baseTypeToken &&
				base.Equals(other));
		}

		/// <summary>
		/// Called to retrieve the namespace of the type.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <returns>The namespace of the type.</returns>
		private string GetNamespace(IMetadataProvider metadataProvider)
		{
			if (IsNested)
			{
				TokenTypes enclosingType = GetEnclosingType(metadataProvider, Token);
				TypeDefRow typeDef = metadataProvider.ReadTypeDefRow(enclosingType);

				string enclosingNamespace = metadataProvider.ReadString(typeDef.TypeNamespaceIdx);
				string enclosingTypeName = metadataProvider.ReadString(typeDef.TypeNameIdx);

				return enclosingNamespace + "." + enclosingTypeName;
			}
			else
			{
				return metadataProvider.ReadString(this.namespaceIdx);
			}
		}

		private TokenTypes GetEnclosingType(IMetadataProvider metadataProvider, int token)
		{
			for (int i = 1; ; i++)
			{
				NestedClassRow row = metadataProvider.ReadNestedClassRow(TokenTypes.NestedClass + i);
				if (row.NestedClassTableIdx == (TokenTypes)token)
					return row.EnclosingClassTableIdx;
			}
		}

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
