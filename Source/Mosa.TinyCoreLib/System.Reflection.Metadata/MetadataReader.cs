using System.Collections.Immutable;

namespace System.Reflection.Metadata;

public sealed class MetadataReader
{
	public AssemblyFileHandleCollection AssemblyFiles
	{
		get
		{
			throw null;
		}
	}

	public AssemblyReferenceHandleCollection AssemblyReferences
	{
		get
		{
			throw null;
		}
	}

	public CustomAttributeHandleCollection CustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public CustomDebugInformationHandleCollection CustomDebugInformation
	{
		get
		{
			throw null;
		}
	}

	public DebugMetadataHeader? DebugMetadataHeader
	{
		get
		{
			throw null;
		}
	}

	public DeclarativeSecurityAttributeHandleCollection DeclarativeSecurityAttributes
	{
		get
		{
			throw null;
		}
	}

	public DocumentHandleCollection Documents
	{
		get
		{
			throw null;
		}
	}

	public EventDefinitionHandleCollection EventDefinitions
	{
		get
		{
			throw null;
		}
	}

	public ExportedTypeHandleCollection ExportedTypes
	{
		get
		{
			throw null;
		}
	}

	public FieldDefinitionHandleCollection FieldDefinitions
	{
		get
		{
			throw null;
		}
	}

	public ImportScopeCollection ImportScopes
	{
		get
		{
			throw null;
		}
	}

	public bool IsAssembly
	{
		get
		{
			throw null;
		}
	}

	public LocalConstantHandleCollection LocalConstants
	{
		get
		{
			throw null;
		}
	}

	public LocalScopeHandleCollection LocalScopes
	{
		get
		{
			throw null;
		}
	}

	public LocalVariableHandleCollection LocalVariables
	{
		get
		{
			throw null;
		}
	}

	public ManifestResourceHandleCollection ManifestResources
	{
		get
		{
			throw null;
		}
	}

	public MemberReferenceHandleCollection MemberReferences
	{
		get
		{
			throw null;
		}
	}

	public MetadataKind MetadataKind
	{
		get
		{
			throw null;
		}
	}

	public int MetadataLength
	{
		get
		{
			throw null;
		}
	}

	public unsafe byte* MetadataPointer
	{
		get
		{
			throw null;
		}
	}

	public string MetadataVersion
	{
		get
		{
			throw null;
		}
	}

	public MethodDebugInformationHandleCollection MethodDebugInformation
	{
		get
		{
			throw null;
		}
	}

	public MethodDefinitionHandleCollection MethodDefinitions
	{
		get
		{
			throw null;
		}
	}

	public MetadataReaderOptions Options
	{
		get
		{
			throw null;
		}
	}

	public PropertyDefinitionHandleCollection PropertyDefinitions
	{
		get
		{
			throw null;
		}
	}

	public MetadataStringComparer StringComparer
	{
		get
		{
			throw null;
		}
	}

	public TypeDefinitionHandleCollection TypeDefinitions
	{
		get
		{
			throw null;
		}
	}

	public TypeReferenceHandleCollection TypeReferences
	{
		get
		{
			throw null;
		}
	}

	public MetadataStringDecoder UTF8Decoder
	{
		get
		{
			throw null;
		}
	}

	public unsafe MetadataReader(byte* metadata, int length)
	{
	}

	public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options)
	{
	}

	public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options, MetadataStringDecoder? utf8Decoder)
	{
	}

	public AssemblyDefinition GetAssemblyDefinition()
	{
		throw null;
	}

	public AssemblyFile GetAssemblyFile(AssemblyFileHandle handle)
	{
		throw null;
	}

	public static AssemblyName GetAssemblyName(string assemblyFile)
	{
		throw null;
	}

	public AssemblyReference GetAssemblyReference(AssemblyReferenceHandle handle)
	{
		throw null;
	}

	public byte[] GetBlobBytes(BlobHandle handle)
	{
		throw null;
	}

	public ImmutableArray<byte> GetBlobContent(BlobHandle handle)
	{
		throw null;
	}

	public BlobReader GetBlobReader(BlobHandle handle)
	{
		throw null;
	}

	public BlobReader GetBlobReader(StringHandle handle)
	{
		throw null;
	}

	public Constant GetConstant(ConstantHandle handle)
	{
		throw null;
	}

	public CustomAttribute GetCustomAttribute(CustomAttributeHandle handle)
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes(EntityHandle handle)
	{
		throw null;
	}

	public CustomDebugInformation GetCustomDebugInformation(CustomDebugInformationHandle handle)
	{
		throw null;
	}

	public CustomDebugInformationHandleCollection GetCustomDebugInformation(EntityHandle handle)
	{
		throw null;
	}

	public DeclarativeSecurityAttribute GetDeclarativeSecurityAttribute(DeclarativeSecurityAttributeHandle handle)
	{
		throw null;
	}

	public Document GetDocument(DocumentHandle handle)
	{
		throw null;
	}

	public EventDefinition GetEventDefinition(EventDefinitionHandle handle)
	{
		throw null;
	}

	public ExportedType GetExportedType(ExportedTypeHandle handle)
	{
		throw null;
	}

	public FieldDefinition GetFieldDefinition(FieldDefinitionHandle handle)
	{
		throw null;
	}

	public GenericParameter GetGenericParameter(GenericParameterHandle handle)
	{
		throw null;
	}

	public GenericParameterConstraint GetGenericParameterConstraint(GenericParameterConstraintHandle handle)
	{
		throw null;
	}

	public Guid GetGuid(GuidHandle handle)
	{
		throw null;
	}

	public ImportScope GetImportScope(ImportScopeHandle handle)
	{
		throw null;
	}

	public InterfaceImplementation GetInterfaceImplementation(InterfaceImplementationHandle handle)
	{
		throw null;
	}

	public LocalConstant GetLocalConstant(LocalConstantHandle handle)
	{
		throw null;
	}

	public LocalScope GetLocalScope(LocalScopeHandle handle)
	{
		throw null;
	}

	public LocalScopeHandleCollection GetLocalScopes(MethodDebugInformationHandle handle)
	{
		throw null;
	}

	public LocalScopeHandleCollection GetLocalScopes(MethodDefinitionHandle handle)
	{
		throw null;
	}

	public LocalVariable GetLocalVariable(LocalVariableHandle handle)
	{
		throw null;
	}

	public ManifestResource GetManifestResource(ManifestResourceHandle handle)
	{
		throw null;
	}

	public MemberReference GetMemberReference(MemberReferenceHandle handle)
	{
		throw null;
	}

	public MethodDebugInformation GetMethodDebugInformation(MethodDebugInformationHandle handle)
	{
		throw null;
	}

	public MethodDebugInformation GetMethodDebugInformation(MethodDefinitionHandle handle)
	{
		throw null;
	}

	public MethodDefinition GetMethodDefinition(MethodDefinitionHandle handle)
	{
		throw null;
	}

	public MethodImplementation GetMethodImplementation(MethodImplementationHandle handle)
	{
		throw null;
	}

	public MethodSpecification GetMethodSpecification(MethodSpecificationHandle handle)
	{
		throw null;
	}

	public ModuleDefinition GetModuleDefinition()
	{
		throw null;
	}

	public ModuleReference GetModuleReference(ModuleReferenceHandle handle)
	{
		throw null;
	}

	public NamespaceDefinition GetNamespaceDefinition(NamespaceDefinitionHandle handle)
	{
		throw null;
	}

	public NamespaceDefinition GetNamespaceDefinitionRoot()
	{
		throw null;
	}

	public Parameter GetParameter(ParameterHandle handle)
	{
		throw null;
	}

	public PropertyDefinition GetPropertyDefinition(PropertyDefinitionHandle handle)
	{
		throw null;
	}

	public StandaloneSignature GetStandaloneSignature(StandaloneSignatureHandle handle)
	{
		throw null;
	}

	public string GetString(DocumentNameBlobHandle handle)
	{
		throw null;
	}

	public string GetString(NamespaceDefinitionHandle handle)
	{
		throw null;
	}

	public string GetString(StringHandle handle)
	{
		throw null;
	}

	public TypeDefinition GetTypeDefinition(TypeDefinitionHandle handle)
	{
		throw null;
	}

	public TypeReference GetTypeReference(TypeReferenceHandle handle)
	{
		throw null;
	}

	public TypeSpecification GetTypeSpecification(TypeSpecificationHandle handle)
	{
		throw null;
	}

	public string GetUserString(UserStringHandle handle)
	{
		throw null;
	}
}
