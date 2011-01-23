using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem
{
	public class TypeLoader
	{
		#region Data members

		/// <summary>
		/// Holds the metadata provider
		/// </summary>
		private IMetadataProvider metadataProvider;

		/// <summary>
		/// Array of loaded runtime type descriptors.
		/// </summary>
		private RuntimeType[] types;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] methods;

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		private RuntimeParameter[] parameters;

		/// <summary>
		/// Array of loaded runtime typeSpec descriptors.
		/// </summary>
		private RuntimeType[] typeSpecs;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] methodSpecs;

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		private RuntimeField[] fields;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		public TypeLoader(IMetadataProvider metadataProvider)
		{
			Debug.Assert(metadataProvider != null);

			this.metadataProvider = metadataProvider;

			methods = new RuntimeMethod[GetTableRows(TokenTypes.MethodDef)];
			fields = new RuntimeField[GetTableRows(TokenTypes.Field)];
			types = new RuntimeType[GetTableRows(TokenTypes.TypeDef)];
			parameters = new RuntimeParameter[GetTableRows(TokenTypes.Param)];

			typeSpecs = new RuntimeType[GetTableRows(TokenTypes.TypeSpec)];
			methodSpecs = new RuntimeMethod[GetTableRows(TokenTypes.MethodSpec)];

			// Load all types from the assembly into the type array
			LoadTypes();
			//LoadGenerics();
			//LoadTypeSpecs();
			LoadParameters();
			//LoadCustomAttributes();
			LoadInterfaces();
		}

		#endregion // Construction

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TokenTypes tokenType)
		{
			return (int)(TokenTypes.RowIndexMask & metadataProvider.GetMaxTokenValue(tokenType));
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			int typeOffset = 0;
			int methodOffset = 0;
			int fieldOffset = 0;
			int size = 0;
			int packing = 0;

			TokenTypes maxTypeDef = metadataProvider.GetMaxTokenValue(TokenTypes.TypeDef);
			TokenTypes maxLayout = metadataProvider.GetMaxTokenValue(TokenTypes.ClassLayout);
			TokenTypes maxMethod = metadataProvider.GetMaxTokenValue(TokenTypes.MethodDef);
			TokenTypes maxField = metadataProvider.GetMaxTokenValue(TokenTypes.Field);

			TokenTypes tokenLayout = TokenTypes.ClassLayout + 1;
			ClassLayoutRow layoutRow = new ClassLayoutRow();

			if (TokenTypes.ClassLayout < maxLayout)
			{
				layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
			}

			TypeDefRow typeDefRow = metadataProvider.ReadTypeDefRow(TokenTypes.TypeDef + 1);
			TypeDefRow nextTypeDefRow = new TypeDefRow();

			for (TokenTypes token = TokenTypes.TypeDef + 1; token <= maxTypeDef; token++)
			{
				TokenTypes maxNextMethod, maxNextField;
				string name = metadataProvider.ReadString(typeDefRow.TypeNameIdx);

				if (token < maxTypeDef)
				{
					nextTypeDefRow = metadataProvider.ReadTypeDefRow(token + 1);
					maxNextField = nextTypeDefRow.FieldList;
					maxNextMethod = nextTypeDefRow.MethodList;

					if (maxNextMethod > maxMethod)
						maxNextMethod = maxMethod + 1;
					if (maxNextField > maxField)
						maxNextField = maxField + 1;
				}
				else
				{
					maxNextMethod = maxMethod + 1;
					maxNextField = maxField + 1;
				}

				// Is this our layout info?
				if (layoutRow.ParentTypeDefIdx == token)
				{
					size = layoutRow.ClassSize;
					packing = layoutRow.PackingSize;

					tokenLayout++;
					if (tokenLayout <= maxLayout)
						layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
				}

				//string typeName = metadataProvider.ReadString(typeDefRow.TypeNameIdx);
				//string typeNamespace = metadataProvider.ReadString(typeDefRow.TypeNamespaceIdx);

				RuntimeType baseType = (typeDefRow.Extends != TokenTypes.TypeDef) ? types[(int)(typeDefRow.Extends & TokenTypes.RowIndexMask)] : null;

				// Create and populate the runtime type
				CilRuntimeType type = new CilRuntimeType(metadataProvider, token, typeDefRow, packing, size, baseType);
				LoadMethods(type, typeDefRow.MethodList, maxNextMethod, ref methodOffset);
				LoadFields(type, typeDefRow.FieldList, maxNextField, ref fieldOffset);
				types[typeOffset++] = type;

				typeDefRow = nextTypeDefRow;
				packing = 0;
				size = 0;
			}

		}

		/// <summary>
		/// Loads all methods from the given metadata module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first method token to load.</param>
		/// <param name="last">The last method token to load (non-inclusive.)</param>
		/// <param name="offset">The offset into the method table to start loading methods From.</param>
		private void LoadMethods(RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			if (first >= last)
				return;

			MethodDefRow nextMethodDef = new MethodDefRow();

			TokenTypes maxParam, maxMethod = metadataProvider.GetMaxTokenValue(TokenTypes.MethodDef);
			MethodDefRow methodDef = metadataProvider.ReadMethodDefRow(first);

			for (TokenTypes token = first; token < last; token++)
			{
				if (token < maxMethod)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token + 1);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = metadataProvider.GetMaxTokenValue(TokenTypes.Param) + 1;
				}

				Debug.Assert(offset < methods.Length, @"Invalid method index.");

				CilRuntimeMethod method = new CilRuntimeMethod(metadataProvider, offset, methodDef, declaringType);
				declaringType.Methods.Add(method);

				methods[offset++] = method;
				methodDef = nextMethodDef;
			}

		}

		/// <summary>
		/// Loads all parameters from the given metadata module.
		/// </summary>
		private void LoadParameters()
		{
			TokenTypes maxParam = metadataProvider.GetMaxTokenValue(TokenTypes.Param);
			TokenTypes token = TokenTypes.Param + 1;

			int offset = 0;

			while (token <= maxParam)
			{
				ParamRow paramDef = metadataProvider.ReadParamRow(token++);
				parameters[offset++] = new RuntimeParameter(metadataProvider, paramDef);
			}
		}

		/// <summary>
		/// Loads all fields defined in the module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first field token to load.</param>
		/// <param name="last">The last field token to load (non-inclusive.)</param>
		/// <param name="offset">The offset in the fields array.</param>
		private void LoadFields(RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			TokenTypes maxRVA = metadataProvider.GetMaxTokenValue(TokenTypes.FieldRVA);
			TokenTypes maxLayout = metadataProvider.GetMaxTokenValue(TokenTypes.FieldLayout);
			TokenTypes tokenRva = TokenTypes.FieldRVA + 1;
			TokenTypes tokenLayout = TokenTypes.FieldLayout + 1;

			FieldRVARow fieldRVA = new FieldRVARow();
			FieldLayoutRow fieldLayout = new FieldLayoutRow();

			if (TokenTypes.FieldRVA < maxRVA)
			{
				fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva);
			}

			if (TokenTypes.FieldLayout < maxLayout)
			{
				fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
			}

			for (TokenTypes token = first; token < last; token++)
			{
				// Read the stackFrameIndex
				FieldRow fieldRow = metadataProvider.ReadFieldRow(token);
				uint rva = 0;
				uint layout = 0;

				// Static fields have an optional RVA, non-static may have a layout assigned
				if ((fieldRow.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				{
					// Move to the RVA of this field
					while (fieldRVA.FieldTableIdx < token && tokenRva <= maxRVA)
					{
						fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva++);
					}

					// Does this field have an RVA?
					if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
					{
						rva = fieldRVA.Rva;
						tokenRva++;
						if (tokenRva < maxRVA)
						{
							fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva);
						}
					}
				}

				if ((fieldRow.Flags & FieldAttributes.HasDefault) == FieldAttributes.HasDefault)
				{
					// FIXME: Has a default value.
					//Debug.Assert(false);
				}

				// Layout only exists for non-static fields
				if ((fieldRow.Flags & FieldAttributes.Static) != FieldAttributes.Static)
				{
					// Move to the layout of this field
					while (fieldLayout.Field < token && tokenLayout <= maxLayout)
						fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout++);

					// Does this field have layout?
					if (token == fieldLayout.Field && tokenLayout <= maxLayout)
					{
						layout = fieldLayout.Offset;
						tokenLayout++;
						if (tokenLayout < maxLayout)
						{
							fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
						}
					}
				}

				// Load the field metadata
				CilRuntimeField field = new CilRuntimeField(metadataProvider, fieldRow, layout, rva, declaringType);
				declaringType.Fields.Add(field);
				fields[offset++] = field;
			}

			/* FIXME:
			 * Load FieldMarshal tables
			 * as needed afterwards. All Generics have been loaded, fields can receive
			 * their signature in the load method above.
			 */
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadInterfaces()
		{
			TokenTypes maxToken = metadataProvider.GetMaxTokenValue(TokenTypes.InterfaceImpl);
			for (TokenTypes token = TokenTypes.InterfaceImpl + 1; token <= maxToken; token++)
			{
				InterfaceImplRow row = metadataProvider.ReadInterfaceImplRow(token);

				RuntimeType interfaceType = types[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask)];
				RuntimeType declaringType = types[(int)(row.InterfaceTableIdx & TokenTypes.RowIndexMask)];

				declaringType.Interfaces.Add(interfaceType);
			}

		}

	}
}
