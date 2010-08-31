/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Loader
{

	/// <summary>
	/// The MergedMetadata class consolidates multiple IMetadataModule provides into a single IMetadataModule view.
	/// </summary>
	public class MergedMetadata : IMetadataProvider, IMetadataModule
	{
		#region Constants

		/// <summary>
		/// Signature constant of the provider root.
		/// </summary>
		private const uint MaxTables = 45;

		/// <summary>
		/// Shift value for tables in TokenTypes Enum
		/// </summary>
		private const byte TableTokenTypeShift = 24;

		#endregion // Constants

		#region Data members

		protected struct ModuleOffset
		{
			public uint Start;
			public uint Count;

			public uint End { get { return Start + Count; } }

			public ModuleOffset(uint start, uint count)
			{
				Start = start;
				Count = count;
			}

			public bool IsWithin(uint value)
			{
				return ((value >= Start) && (value < End));
			}

		}

		protected IMetadataModule[] modules;
		protected ModuleOffset[][] moduleOffset;
		protected ModuleOffset[][] moduleStreamOffset;
		protected List<string> codeBases;
		protected TokenTypes entryPoint;
		protected ModuleType moduleType;
		protected List<string> names;
		protected int loadOrder = -1;

		#endregion // Data members

		/// <summary>
		/// Initializes a new instance of the <see cref="MergedMetadata"/> class.
		/// </summary>
		/// <param name="modules">The modules.</param>
		public MergedMetadata(IList<IMetadataModule> modules)
		{
			Initialize(modules);
		}

		#region IMetadataModule members

		/// <summary>
		/// Provides access to the provider contained in the assembly.
		/// </summary>
		IMetadataProvider IMetadataModule.Metadata { get { return this; } }

		/// <summary>
		/// Retrieves the name of the module.
		/// </summary>
		IList<string> IMetadataModule.Names { get { return names; } }

		/// <summary>
		/// Provides access to the sequence of IL opcodes for a relative
		/// virtual address.
		/// </summary>
		/// <param name="rva">The relative virtual address to retrieve a stream for.</param>
		/// <returns>A stream, which represents the relative virtual address.</returns>
		Stream IMetadataModule.GetInstructionStream(long rva)
		{
			uint module;
			ulong originalrva = (ulong)rva;

			GetOriginalRVA(out module, ref originalrva);

			return modules[module].GetInstructionStream((long)originalrva);
		}

		/// <summary>
		/// Gets a stream into the data section, beginning at the specified RVA.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <returns>A stream into the data section, pointed to the requested RVA.</returns>
		Stream IMetadataModule.GetDataSection(long rva)
		{
			uint module;
			ulong originalrva = (ulong)rva;

			GetOriginalRVA(out module, ref originalrva);

			return modules[module].GetDataSection((long)originalrva);
		}

		/// <summary>
		/// Gets the code base of the module.
		/// </summary>
		/// <value>The code base of the module.</value>
		IList<string> IMetadataModule.CodeBases { get { return codeBases; } }

		/// <summary>
		/// Gets the entry point of the module.
		/// </summary>
		/// <value>The entry point.</value>
		TokenTypes IMetadataModule.EntryPoint { get { return entryPoint; } }

		/// <summary>
		/// Retrieves the load order index of the module.
		/// </summary>
		int IMetadataModule.LoadOrder { get { return loadOrder; } set { loadOrder = value; } }

		/// <summary>
		/// Gets the type of the module.
		/// </summary>
		/// <value>The type of the module.</value>
		ModuleType IMetadataModule.ModuleType { get { return moduleType; } }

		#endregion // IMetadataModule members

		#region Methods

		protected void Initialize(IList<IMetadataModule> modules)
		{
			this.modules = new IMetadataModule[modules.Count];
			moduleOffset = new ModuleOffset[modules.Count][];
			moduleStreamOffset = new ModuleOffset[4][];
			moduleType = ModuleType.Library;
			codeBases = new List<string>();
			names = new List<string>();

			for (uint mod = 0; mod < modules.Count; mod++)
			{
				IMetadataModule module = modules[(int)mod];
				this.modules[mod] = module;

				foreach (string code in module.CodeBases)
					codeBases.Add(code);

				foreach (string name in module.Names)
					names.Add(name);

				moduleOffset[mod] = new ModuleOffset[MaxTables];
				moduleStreamOffset[mod] = new ModuleOffset[4];

				for (int table = 0; table < MaxTables; table++)
				{
					uint previous = (mod == 0 ? 1 : moduleOffset[mod - 1][table].End);

					TokenTypes entries = module.Metadata.GetMaxTokenValue((TokenTypes)(table << TableTokenTypeShift));

					moduleOffset[mod][table] = new ModuleOffset(previous, (uint)(TokenTypes.RowIndexMask & entries));
				}

				for (int table = 0; table < 4; table++)
				{
					uint previous = (mod == 0 ? 0 : moduleStreamOffset[mod - 1][table].Start + moduleStreamOffset[mod - 1][table].Count);

					TokenTypes entries = module.Metadata.GetMaxTokenValue((TokenTypes)((table << TableTokenTypeShift) + TokenTypes.UserString));

					moduleStreamOffset[mod][table] = new ModuleOffset(previous, (uint)(TokenTypes.RowIndexMask & entries));
				}

				if (module.ModuleType == ModuleType.Executable)
				{
					moduleType = ModuleType.Executable;
				}

				if (module.EntryPoint != 0)
				{
					entryPoint = GetNewToken(mod, module.EntryPoint);
				}
			}

		}

		protected void GetModuleOffset(TokenTypes token, out uint module, out uint index)
		{
			if (token < TokenTypes.MaxTable)
			{
				uint table = ((uint)(token & TokenTypes.TableMask) >> TableTokenTypeShift);
				uint rowindex = (uint)(token & TokenTypes.RowIndexMask);

				for (uint mod = 0; mod < modules.Length; mod++)
					if (moduleOffset[mod][table].IsWithin(rowindex))
					{
						module = mod;
						index = rowindex - moduleOffset[mod][table].Start + 1;
						return;
					}
			}
			else if (token >= TokenTypes.UserString && token < TokenTypes.MaxHeap)
			{
				uint table = ((uint)((token & TokenTypes.TableMask) - TokenTypes.UserString) >> TableTokenTypeShift);
				uint rowindex = (uint)(token & TokenTypes.RowIndexMask);

				for (uint mod = 0; mod < modules.Length; mod++)
					if (moduleStreamOffset[mod][table].IsWithin(rowindex))
					{
						module = mod;
						index = rowindex - moduleStreamOffset[mod][table].Start;
						return;
					}
			}

			throw new ArgumentException(@"Not a valid tokentype.", @"token");
		}

		protected TokenTypes GetOriginalToken(TokenTypes token, out uint module)
		{
			uint index;

			GetModuleOffset(token, out module, out index);

			return (TokenTypes)((token & TokenTypes.TableMask) + (int)index);
		}

		protected TokenTypes GetNewToken(uint module, TokenTypes token)
		{
			if (token < TokenTypes.MaxTable)
			{
				if ((uint)(token & TokenTypes.RowIndexMask) == 0)
					return (token & TokenTypes.TableMask);

				uint table = ((uint)(token & TokenTypes.TableMask) >> TableTokenTypeShift);
				ulong offset = moduleOffset[module][table].Start;

				return (TokenTypes)(token + (int)offset - 1);
			}
			else if (token >= TokenTypes.UserString && token < TokenTypes.MaxHeap)
			{
				if ((uint)(token & TokenTypes.RowIndexMask) == 0)
					return (token & TokenTypes.TableMask);

				uint table = ((uint)((token & TokenTypes.TableMask) - TokenTypes.UserString) >> TableTokenTypeShift);
				ulong offset = moduleStreamOffset[module][table].Start;

				return (TokenTypes)(token + (int)offset);
			}

			throw new ArgumentException(@"Not a valid tokentype.", @"token");
		}

		protected uint GetMaxTokenCount(TokenTypes token)
		{
			if (token < TokenTypes.MaxTable)
			{
				return (uint)(moduleOffset[modules.Length - 1][((uint)token) >> TableTokenTypeShift].End);
			}
			else if (token >= TokenTypes.UserString && token <= TokenTypes.Guid)
			{
				return (uint)(moduleStreamOffset[modules.Length - 1][((uint)((token & TokenTypes.TableMask) - TokenTypes.UserString) >> TableTokenTypeShift)].End);
			}

			throw new ArgumentException(@"Not a valid tokentype.", @"token");
		}

		protected void GetOriginalRVA(out uint module, ref ulong rva)
		{
			module = (uint)(rva >> 60);
			rva = rva & 0x0FFFFFFFFFFFFFFF;
		}

		protected ulong GetNewRVA(uint module, ulong rva)
		{
			ulong newrva = module;
			newrva = newrva << 60;
			newrva = newrva | rva;
			return newrva;
		}

		protected int GetModuleIndex(IMetadataModule module)
		{
			for (int i = 0; i < modules.Length; i++)
				if (modules[i] == module)
					return i;

			throw new ArgumentException(@"Unable to locate module.", @"module");
		}

		#endregion // Methods

		#region IMetadataProvider members

		TokenTypes IMetadataProvider.GetMaxTokenValue(TokenTypes token)
		{
			return (TokenTypes)(GetMaxTokenCount(token) - 1) | (token & TokenTypes.TableMask);
		}

		string IMetadataProvider.ReadString(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			return modules[module].Metadata.ReadString(originalToken);
		}

		Guid IMetadataProvider.ReadGuid(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			return modules[module].Metadata.ReadGuid(originalToken);
		}

		byte[] IMetadataProvider.ReadBlob(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			return modules[module].Metadata.ReadBlob(originalToken);
		}

		ModuleRow IMetadataProvider.ReadModuleRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ModuleRow row = modules[module].Metadata.ReadModuleRow(originalToken);
			return new ModuleRow(
				row.Generation,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.MvidGuidIdx),
				GetNewToken(module, row.EncIdGuidIdx),
				GetNewToken(module, row.EncBaseIdGuidIdx)
			);
		}

		TypeRefRow IMetadataProvider.ReadTypeRefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			TypeRefRow row = modules[module].Metadata.ReadTypeRefRow(originalToken);
			return new TypeRefRow(
				GetNewToken(module, row.ResolutionScopeIdx),
				GetNewToken(module, row.TypeNameIdx),
				GetNewToken(module, row.TypeNamespaceIdx)
			);
		}

		TypeDefRow IMetadataProvider.ReadTypeDefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			TypeDefRow row = modules[module].Metadata.ReadTypeDefRow(originalToken);
			return new TypeDefRow(
				row.Flags,
				GetNewToken(module, row.TypeNameIdx),
				GetNewToken(module, row.TypeNamespaceIdx),
				GetNewToken(module, row.Extends),
				GetNewToken(module, row.FieldList),
				GetNewToken(module, row.MethodList)
			);
		}

		FieldRow IMetadataProvider.ReadFieldRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			FieldRow row = modules[module].Metadata.ReadFieldRow(originalToken);
			return new FieldRow(
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.SignatureBlobIdx)
				);
		}

		MethodDefRow IMetadataProvider.ReadMethodDefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			MethodDefRow row = modules[module].Metadata.ReadMethodDefRow(originalToken);
			return new MethodDefRow(
				GetNewRVA(module, row.Rva),
				row.ImplFlags,
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.SignatureBlobIdx),
				GetNewToken(module, row.ParamList)
			);
		}

		ParamRow IMetadataProvider.ReadParamRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ParamRow row = modules[module].Metadata.ReadParamRow(originalToken);
			return new ParamRow(
				row.Flags,
				row.Sequence,
				GetNewToken(module, row.NameIdx)
			);
		}

		InterfaceImplRow IMetadataProvider.ReadInterfaceImplRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			InterfaceImplRow row = modules[module].Metadata.ReadInterfaceImplRow(originalToken);
			return new InterfaceImplRow(
				GetNewToken(module, row.ClassTableIdx),
				GetNewToken(module, row.InterfaceTableIdx)
			);
		}

		MemberRefRow IMetadataProvider.ReadMemberRefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			MemberRefRow row = modules[module].Metadata.ReadMemberRefRow(originalToken);
			return new MemberRefRow(
				GetNewToken(module, row.ClassTableIdx),
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.SignatureBlobIdx)
			);
		}

		ConstantRow IMetadataProvider.ReadConstantRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ConstantRow row = modules[module].Metadata.ReadConstantRow(originalToken);
			return new ConstantRow(
				row.Type,
				GetNewToken(module, row.Parent),
				GetNewToken(module, row.ValueBlobIdx)
			);
		}

		CustomAttributeRow IMetadataProvider.ReadCustomAttributeRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			CustomAttributeRow row = modules[module].Metadata.ReadCustomAttributeRow(originalToken);
			return new CustomAttributeRow(
				GetNewToken(module, row.ParentTableIdx),
				GetNewToken(module, row.TypeIdx),
				GetNewToken(module, row.ValueBlobIdx)
			);
		}

		FieldMarshalRow IMetadataProvider.ReadFieldMarshalRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			FieldMarshalRow row = modules[module].Metadata.ReadFieldMarshalRow(originalToken);
			return new FieldMarshalRow(
				GetNewToken(module, row.ParentTableIdx),
				GetNewToken(module, row.NativeTypeBlobIdx)
			);
		}

		DeclSecurityRow IMetadataProvider.ReadDeclSecurityRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			DeclSecurityRow row = modules[module].Metadata.ReadDeclSecurityRow(originalToken);
			return new DeclSecurityRow(
				row.Action,
				GetNewToken(module, row.ParentTableIdx),
				GetNewToken(module, row.PermissionSetBlobIdx)
			);
		}

		ClassLayoutRow IMetadataProvider.ReadClassLayoutRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ClassLayoutRow row = modules[module].Metadata.ReadClassLayoutRow(originalToken);
			return new ClassLayoutRow(
				row.PackingSize,
				row.ClassSize,
				GetNewToken(module, row.ParentTypeDefIdx)
			);
		}

		FieldLayoutRow IMetadataProvider.ReadFieldLayoutRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			FieldLayoutRow row = modules[module].Metadata.ReadFieldLayoutRow(originalToken);
			return new FieldLayoutRow(
				row.Offset,
				GetNewToken(module, row.Field)
			);
		}

		StandAloneSigRow IMetadataProvider.ReadStandAloneSigRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			StandAloneSigRow row = modules[module].Metadata.ReadStandAloneSigRow(originalToken);
			return new StandAloneSigRow(
				GetNewToken(module, row.SignatureBlobIdx)
			);
		}

		EventMapRow IMetadataProvider.ReadEventMapRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			EventMapRow row = modules[module].Metadata.ReadEventMapRow(originalToken);
			return new EventMapRow(
				GetNewToken(module, row.TypeDefTableIdx),
				GetNewToken(module, row.EventListTableIdx)
			);
		}

		EventRow IMetadataProvider.ReadEventRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			EventRow row = modules[module].Metadata.ReadEventRow(originalToken);
			return new EventRow(
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.EventTypeTableIdx)
			);
		}

		PropertyMapRow IMetadataProvider.ReadPropertyMapRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			PropertyMapRow row = modules[module].Metadata.ReadPropertyMapRow(originalToken);
			return new PropertyMapRow(
				GetNewToken(module, row.ParentTableIdx),
				GetNewToken(module, row.PropertyTableIdx)
			);
		}

		PropertyRow IMetadataProvider.ReadPropertyRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			PropertyRow row = modules[module].Metadata.ReadPropertyRow(originalToken);
			return new PropertyRow(
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.TypeBlobIdx)
			);
		}

		MethodSemanticsRow IMetadataProvider.ReadMethodSemanticsRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			MethodSemanticsRow row = modules[module].Metadata.ReadMethodSemanticsRow(originalToken);
			return new MethodSemanticsRow(
				row.Semantics,
			GetNewToken(module, row.MethodTableIdx),
			GetNewToken(module, row.AssociationTableIdx)
			);
		}

		MethodImplRow IMetadataProvider.ReadMethodImplRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			MethodImplRow row = modules[module].Metadata.ReadMethodImplRow(originalToken);
			return new MethodImplRow(
				GetNewToken(module, row.ClassTableIdx),
				GetNewToken(module, row.MethodBodyTableIdx),
				GetNewToken(module, row.MethodDeclarationTableIdx)
			);
		}

		ModuleRefRow IMetadataProvider.ReadModuleRefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ModuleRefRow row = modules[module].Metadata.ReadModuleRefRow(originalToken);
			return new ModuleRefRow(
				GetNewToken(module, row.NameStringIdx)
			);
		}

		TypeSpecRow IMetadataProvider.ReadTypeSpecRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			TypeSpecRow row = modules[module].Metadata.ReadTypeSpecRow(originalToken);
			return new TypeSpecRow(
				GetNewToken(module, row.SignatureBlobIdx)
			);
		}

		ImplMapRow IMetadataProvider.ReadImplMapRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ImplMapRow row = modules[module].Metadata.ReadImplMapRow(originalToken);
			return new ImplMapRow(
				row.MappingFlags,
			GetNewToken(module, row.MemberForwardedTableIdx),
			GetNewToken(module, row.ImportNameStringIdx),
			GetNewToken(module, row.ImportScopeTableIdx)
			);
		}

		FieldRVARow IMetadataProvider.ReadFieldRVARow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			FieldRVARow row = modules[module].Metadata.ReadFieldRVARow(originalToken);
			return new FieldRVARow(
				GetNewRVA(module, row.Rva),
				GetNewToken(module, row.FieldTableIdx)
			);
		}

		AssemblyRow IMetadataProvider.ReadAssemblyRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyRow row = modules[module].Metadata.ReadAssemblyRow(originalToken);
			return new AssemblyRow(
				row.HashAlgId,
				row.MajorVersion,
				row.MinorVersion,
				row.BuildNumber,
				row.Revision,
				row.Flags,
				GetNewToken(module, row.PublicKeyIdx),
				GetNewToken(module, row.NameIdx),
				GetNewToken(module, row.CultureIdx)
			);
		}

		AssemblyProcessorRow IMetadataProvider.ReadAssemblyProcessorRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyProcessorRow row = modules[module].Metadata.ReadAssemblyProcessorRow(originalToken);
			return row; // no change
		}

		AssemblyOSRow IMetadataProvider.ReadAssemblyOSRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyOSRow row = modules[module].Metadata.ReadAssemblyOSRow(originalToken);
			return row; // no change
		}

		AssemblyRefRow IMetadataProvider.ReadAssemblyRefRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyRefRow row = modules[module].Metadata.ReadAssemblyRefRow(originalToken);
			return new AssemblyRefRow(
				row.MajorVersion,
				row.MinorVersion,
				row.BuildNumber,
				row.Revision,
				row.Flags,
				GetNewToken(module, row.PublicKeyOrTokenIdx),
				GetNewToken(module, row.NameIdx),
				GetNewToken(module, row.CultureIdx),
				GetNewToken(module, row.HashValueIdx)
			);
		}

		AssemblyRefProcessorRow IMetadataProvider.ReadAssemblyRefProcessorRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyRefProcessorRow row = modules[module].Metadata.ReadAssemblyRefProcessorRow(originalToken);
			return new AssemblyRefProcessorRow(
				row.Processor,
			GetNewToken(module, row.AssemblyRef)
			);
		}

		AssemblyRefOSRow IMetadataProvider.ReadAssemblyRefOSRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			AssemblyRefOSRow row = modules[module].Metadata.ReadAssemblyRefOSRow(originalToken);
			return new AssemblyRefOSRow(
				row.PlatformId,
				row.MajorVersion,
				row.MinorVersion,
				GetNewToken(module, row.AssemblyRefIdx)
			);
		}

		FileRow IMetadataProvider.ReadFileRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			FileRow row = modules[module].Metadata.ReadFileRow(originalToken);
			return new FileRow(
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.HashValueBlobIdx)
			);
		}

		ExportedTypeRow IMetadataProvider.ReadExportedTypeRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ExportedTypeRow row = modules[module].Metadata.ReadExportedTypeRow(originalToken);
			return new ExportedTypeRow(
				row.Flags,
				GetNewToken(module, row.TypeDefTableIdx),
				GetNewToken(module, row.TypeNameStringIdx),
				GetNewToken(module, row.TypeNamespaceStringIdx),
				GetNewToken(module, row.ImplementationTableIdx)
			);
		}

		ManifestResourceRow IMetadataProvider.ReadManifestResourceRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			ManifestResourceRow row = modules[module].Metadata.ReadManifestResourceRow(originalToken);
			return new ManifestResourceRow(
				row.Offset,
				row.Flags,
				GetNewToken(module, row.NameStringIdx),
				GetNewToken(module, row.ImplementationTableIdx)
			);
		}

		NestedClassRow IMetadataProvider.ReadNestedClassRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			NestedClassRow row = modules[module].Metadata.ReadNestedClassRow(originalToken);
			return new NestedClassRow(
				GetNewToken(module, row.NestedClassTableIdx),
				GetNewToken(module, row.EnclosingClassTableIdx)
			);
		}

		GenericParamRow IMetadataProvider.ReadGenericParamRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			GenericParamRow row = modules[module].Metadata.ReadGenericParamRow(originalToken);
			return new GenericParamRow(
				row.Number,
				row.Flags,
				GetNewToken(module, row.OwnerTableIdx),
				GetNewToken(module, row.NameStringIdx)
			);
		}

		MethodSpecRow IMetadataProvider.ReadMethodSpecRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			MethodSpecRow row = modules[module].Metadata.ReadMethodSpecRow(originalToken);
			return new MethodSpecRow(
				GetNewToken(module, row.MethodTableIdx),
				GetNewToken(module, row.InstantiationBlobIdx)
			);
		}

		GenericParamConstraintRow IMetadataProvider.ReadGenericParamConstraintRow(TokenTypes token)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			GenericParamConstraintRow row = modules[module].Metadata.ReadGenericParamConstraintRow(originalToken);
			return new GenericParamConstraintRow(
				GetNewToken(module, row.OwnerTableIdx),
				GetNewToken(module, row.ConstraintTableIdx)
			);
		}

		TokenTypes IMetadataProvider.ApplyTokenTypeAdjustmentByRVA(TokenTypes token, ulong rva)
		{
			uint module = (uint)(rva >> 60);

			TokenTypes newToken = GetNewToken(module, token);

			return newToken;
		}

		TokenTypes IMetadataProvider.ApplyTokenTypeAdjustmentByBlobToken(TokenTypes token, TokenTypes blob)
		{
			uint module;
			TokenTypes originalToken = GetOriginalToken(token, out module);

			TokenTypes newToken = GetNewToken(module, token);

			return newToken;
		}

		/// <summary>
		/// Gets the heaps of a specified type
		/// </summary>
		/// <param name="heapType">Type of the heap.</param>
		/// <returns></returns>
		IList<Heap> IMetadataProvider.GetHeaps(HeapType heapType)
		{
			List<Heap> list = new List<Heap>();

			foreach (IMetadataModule module in modules)
				foreach (Heap heap in module.Metadata.GetHeaps(heapType))
					list.Add(heap);

			return list;
		}

		#endregion // IMetadataProvider members

	}
}
