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

using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Runtime
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
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="token">The token.</param>
		/// <param name="typeDefRow">The type def row.</param>
		/// <param name="maxField">The max field.</param>
		/// <param name="maxMethod">The max method.</param>
		/// <param name="packing">The packing.</param>
		/// <param name="size">The size.</param>
		public CilRuntimeType(IModuleTypeSystem moduleTypeSystem, TokenTypes token, TypeDefRow typeDefRow, TokenTypes maxField, TokenTypes maxMethod, int packing, int size) :
			base(moduleTypeSystem, (int)token)
		{
			this.baseTypeToken = typeDefRow.Extends;
			this.nameIdx = typeDefRow.TypeNameIdx;
			this.namespaceIdx = typeDefRow.TypeNamespaceIdx;

			base.Attributes = typeDefRow.Flags;
			base.Pack = packing;
			base.Size = size;

			// Load all fields of the type
			int members = maxField - typeDefRow.FieldList;
			if (0 < members)
			{
				int i = (int)(typeDefRow.FieldList & TokenTypes.RowIndexMask) - 1 ;
				base.Fields = new ReadOnlyRuntimeFieldListView(moduleTypeSystem,i, members);
			}
			else
			{
				base.Fields = ReadOnlyRuntimeFieldListView.Empty;
			}

			// Load all methods of the type
			members = maxMethod - typeDefRow.MethodList;
			if (0 < members)
			{
				int i = (int)(typeDefRow.MethodList & TokenTypes.RowIndexMask) - 1;
				base.Methods = new ReadOnlyRuntimeMethodListView(moduleTypeSystem, i, members);
			}
			else
			{
				base.Methods = ReadOnlyRuntimeMethodListView.Empty;
			}
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
					this.moduleTypeSystem == crt.moduleTypeSystem &&
					this.nameIdx == crt.nameIdx &&
					this.namespaceIdx == crt.namespaceIdx &&
					this.baseTypeToken == crt.baseTypeToken &&
					base.Equals(other) == true);
		}

		/// <summary>
		/// Gets the base type.
		/// </summary>
		/// <returns>The base type.</returns>
		protected override RuntimeType GetBaseType()
		{
			return moduleTypeSystem.GetType(this, this.baseTypeToken);
		}

		/// <summary>
		/// Called to retrieve the name of the type.
		/// </summary>
		/// <returns>The name of the type.</returns>
		protected override string GetName()
		{
			string name = moduleTypeSystem.MetadataModule.Metadata.ReadString(this.nameIdx);
			Debug.Assert(name != null, @"Failed to retrieve CilRuntimeMethod name.");
			return name;
		}

		/// <summary>
		/// Called to retrieve the namespace of the type.
		/// </summary>
		/// <returns>The namespace of the type.</returns>
		protected override string GetNamespace()
		{
			string nameSpace;
			if (IsNested)
			{
				TokenTypes enclosingType = GetEnclosingType(Token);
				TypeDefRow typeDef = moduleTypeSystem.MetadataModule.Metadata.ReadTypeDefRow(enclosingType);

				string @enclosingNamespace = moduleTypeSystem.MetadataModule.Metadata.ReadString(typeDef.TypeNamespaceIdx);
				string @enclosingTypeName = moduleTypeSystem.MetadataModule.Metadata.ReadString(typeDef.TypeNameIdx);
				nameSpace = enclosingNamespace + "." + enclosingTypeName;
			}
			else
			{
				nameSpace = moduleTypeSystem.MetadataModule.Metadata.ReadString(this.namespaceIdx);
			}
			Debug.Assert(nameSpace != null, @"Failed to retrieve CilRuntimeMethod name.");
			return nameSpace;
		}

		private TokenTypes GetEnclosingType(int token)
		{
			NestedClassRow row;
			for (int i = 1; ; ++i)
			{
				row = moduleTypeSystem.MetadataModule.Metadata.ReadNestedClassRow(TokenTypes.NestedClass + i);
				if (row.NestedClassTableIdx == (TokenTypes)token)
					break;
			}
			return row.EnclosingClassTableIdx;
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

		protected override IList<RuntimeType> LoadInterfaces()
		{
			List<RuntimeType> result = null;
			IMetadataProvider metadata = moduleTypeSystem.MetadataModule.Metadata;

			TokenTypes maxToken = metadata.GetMaxTokenValue(TokenTypes.InterfaceImpl);
			for (TokenTypes token = TokenTypes.InterfaceImpl + 1; token <= maxToken; token++)
			{
				InterfaceImplRow row = metadata.ReadInterfaceImplRow(token);
				if (row.ClassTableIdx == (TokenTypes)this.Token)
				{
					RuntimeType interfaceType = moduleTypeSystem.GetType(DefaultSignatureContext.Instance, row.InterfaceTableIdx);

					if (result == null)
					{
						result = new List<RuntimeType>();
					}

					result.Add(interfaceType);
				}
			}

			if (result != null)
			{
				return result;
			}

			return NoInterfaces;
		}

		#endregion // Methods
	}
}
