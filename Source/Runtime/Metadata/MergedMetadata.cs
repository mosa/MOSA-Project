/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Loader; // ?

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata
{

	/// <summary>
	/// Metadata root structure according to ISO/IEC 23271:2006 (E), §24.2.1
	/// </summary>
	public class MergedMetadata
	{
		#region Constants

		/// <summary>
		/// Signature constant of the provider root.
		/// </summary>
		private const uint MaxTables = 64;

		/// <summary>
		/// Shift value for tables in TokenTypes Enum
		/// </summary>
		private const byte TableTokenTypeShift = 24;

		#endregion // Constants

		#region Data members

		protected struct ModuleOffset
		{
			public int Start;
			public int End;
			public int Count;

			public ModuleOffset(int start, int count)
			{
				Start = start;
				End = start + count;
				Count = count;
			}
		}

		protected IMetadataModule[] modules;
		protected ModuleOffset[][] moduleOffset;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataRoot"/> class.
		/// </summary>
		public MergedMetadata(IList<IMetadataModule> modules)
		{
			Initialize(modules);
		}

		#region Methods

		protected void Initialize(IList<IMetadataModule> modules)
		{
			this.modules = new IMetadataModule[modules.Count];
			moduleOffset = new ModuleOffset[modules.Count][];

			for (int mod = 0; mod < modules.Count; mod++)
			{
				IMetadataModule module = modules[mod];
				this.modules[mod] = module;

				moduleOffset[mod] = new ModuleOffset[MaxTables];

				for (int table = 0; table < MaxTables; table++)
				{
					int previous = (mod == 0 ? 0 : moduleOffset[mod - 1][table].End);

					TokenTypes entries = module.Metadata.GetMaxTokenValue((TokenTypes)(table << TableTokenTypeShift));

					moduleOffset[mod][table] = new ModuleOffset(previous, (int)(TokenTypes.RowIndexMask & entries));
				}
			}
		}

		protected int GetModuleIndex(IMetadataModule module)
		{
			for (int i = 0; i < modules.Length; i++)
				if (modules[i] == module)
					return i;

			return -1;
		}

		protected bool GetModuleOffset(TokenTypes tokenType, out int module, out int index)
		{
			int table = ((int)(tokenType & TokenTypes.TableMask) >> TableTokenTypeShift);
			int rowindex = (int)(tokenType & TokenTypes.RowIndexMask);

			for (int mod = 0; mod < modules.Length; mod++)
				if ((rowindex > moduleOffset[mod][table].Start) & (rowindex < moduleOffset[mod][table].End))
				{
					module = mod;
					index = rowindex - moduleOffset[mod][table].Start;
					return true;
				}

			module = -1;
			index = -1;
			return false;
		}

		protected TokenTypes GetOriginalToken(TokenTypes tokenType, out int module)
		{
			int index;

			if (GetModuleOffset(tokenType, out module, out index))
			{
				return (TokenTypes)((tokenType & TokenTypes.RowIndexMask) + index);
			}

			return (TokenTypes)0;
		}

		#endregion // Methods

		#region IMetadataProvider members

		public int GetMaxTokenCount(TokenTypes tokenType)
		{
			return moduleOffset[modules.Length - 1][((uint)tokenType) >> TableTokenTypeShift].End;
		}

		public TokenTypes GetMaxTokenValue(TokenTypes tokenType)
		{
			return (TokenTypes)GetMaxTokenCount(tokenType);
		}

		public string GetString(TokenTypes tokenType)
		{
			int module;
			TokenTypes originalToken = GetOriginalToken(tokenType, out module);

			return modules[module].Metadata.ReadString(originalToken);
		}

		//TokenTypes Read(TokenTypes token, out Guid result)
		//{

		//    return token;
		//}

		//TokenTypes Read(TokenTypes token, out byte[] result)
		//{

		//    return token;
		//}

		//void Read(TokenTypes token, out ModuleRow result)
		//{

		//}

		//void Read(TokenTypes token, out TypeRefRow result)
		//{

		//}

		//void Read(TokenTypes token, out TypeDefRow result)
		//{

		//}

		//void Read(TokenTypes token, out FieldRow result)
		//{

		//}

		//void Read(TokenTypes token, out MethodDefRow result)
		//{

		//}

		//void Read(TokenTypes token, out ParamRow result)
		//{

		//}

		//void Read(TokenTypes token, out InterfaceImplRow result)
		//{

		//}

		//void Read(TokenTypes token, out MemberRefRow result)
		//{

		//}

		//void Read(TokenTypes token, out ConstantRow result)
		//{

		//}

		//void Read(TokenTypes token, out CustomAttributeRow result)
		//{

		//}

		//void Read(TokenTypes token, out FieldMarshalRow result)
		//{

		//}

		//void Read(TokenTypes token, out DeclSecurityRow result)
		//{

		//}

		//void Read(TokenTypes token, out ClassLayoutRow result)
		//{

		//}

		//void Read(TokenTypes token, out FieldLayoutRow result)
		//{

		//}

		//void Read(TokenTypes token, out StandAloneSigRow result)
		//{

		//}

		//void Read(TokenTypes token, out EventMapRow result)
		//{

		//}

		//void Read(TokenTypes token, out EventRow result)
		//{

		//}

		//void Read(TokenTypes token, out PropertyMapRow result)
		//{

		//}

		//void Read(TokenTypes token, out PropertyRow result)
		//{

		//}

		//void Read(TokenTypes token, out MethodSemanticsRow result)
		//{

		//}

		//void Read(TokenTypes token, out MethodImplRow result)
		//{

		//}

		//void Read(TokenTypes token, out ModuleRefRow result)
		//{

		//}

		//void Read(TokenTypes token, out TypeSpecRow result)
		//{

		//}

		//void Read(TokenTypes token, out ImplMapRow result)
		//{

		//}

		//void Read(TokenTypes token, out FieldRVARow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyRow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyProcessorRow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyOSRow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyRefRow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyRefProcessorRow result)
		//{

		//}

		//void Read(TokenTypes token, out AssemblyRefOSRow result)
		//{

		//}

		//void Read(TokenTypes token, out FileRow result)
		//{

		//}

		//void Read(TokenTypes token, out ExportedTypeRow result)
		//{

		//}

		//void Read(TokenTypes token, out ManifestResourceRow result)
		//{

		//}

		//void Read(TokenTypes token, out NestedClassRow result)
		//{

		//}

		//void Read(TokenTypes token, out GenericParamRow result)
		//{

		//}

		//void Read(TokenTypes token, out MethodSpecRow result)
		//{

		//}

		//void Read(TokenTypes token, out GenericParamConstraintRow result)
		//{

		//}

		//void Read(TokenTypes token, out FieldRow[] result)
		//{

		//}

		#endregion // IMetadataProvider members
	}
}
