namespace System.Reflection.Metadata.Ecma335;

public static class MetadataTokens
{
	public static readonly int HeapCount;

	public static readonly int TableCount;

	public static AssemblyFileHandle AssemblyFileHandle(int rowNumber)
	{
		throw null;
	}

	public static AssemblyReferenceHandle AssemblyReferenceHandle(int rowNumber)
	{
		throw null;
	}

	public static BlobHandle BlobHandle(int offset)
	{
		throw null;
	}

	public static ConstantHandle ConstantHandle(int rowNumber)
	{
		throw null;
	}

	public static CustomAttributeHandle CustomAttributeHandle(int rowNumber)
	{
		throw null;
	}

	public static CustomDebugInformationHandle CustomDebugInformationHandle(int rowNumber)
	{
		throw null;
	}

	public static DeclarativeSecurityAttributeHandle DeclarativeSecurityAttributeHandle(int rowNumber)
	{
		throw null;
	}

	public static DocumentHandle DocumentHandle(int rowNumber)
	{
		throw null;
	}

	public static DocumentNameBlobHandle DocumentNameBlobHandle(int offset)
	{
		throw null;
	}

	public static EntityHandle EntityHandle(int token)
	{
		throw null;
	}

	public static EntityHandle EntityHandle(TableIndex tableIndex, int rowNumber)
	{
		throw null;
	}

	public static EventDefinitionHandle EventDefinitionHandle(int rowNumber)
	{
		throw null;
	}

	public static ExportedTypeHandle ExportedTypeHandle(int rowNumber)
	{
		throw null;
	}

	public static FieldDefinitionHandle FieldDefinitionHandle(int rowNumber)
	{
		throw null;
	}

	public static GenericParameterConstraintHandle GenericParameterConstraintHandle(int rowNumber)
	{
		throw null;
	}

	public static GenericParameterHandle GenericParameterHandle(int rowNumber)
	{
		throw null;
	}

	public static int GetHeapOffset(BlobHandle handle)
	{
		throw null;
	}

	public static int GetHeapOffset(GuidHandle handle)
	{
		throw null;
	}

	public static int GetHeapOffset(Handle handle)
	{
		throw null;
	}

	public static int GetHeapOffset(this MetadataReader reader, Handle handle)
	{
		throw null;
	}

	public static int GetHeapOffset(StringHandle handle)
	{
		throw null;
	}

	public static int GetHeapOffset(UserStringHandle handle)
	{
		throw null;
	}

	public static int GetRowNumber(EntityHandle handle)
	{
		throw null;
	}

	public static int GetRowNumber(this MetadataReader reader, EntityHandle handle)
	{
		throw null;
	}

	public static int GetToken(EntityHandle handle)
	{
		throw null;
	}

	public static int GetToken(Handle handle)
	{
		throw null;
	}

	public static int GetToken(this MetadataReader reader, EntityHandle handle)
	{
		throw null;
	}

	public static int GetToken(this MetadataReader reader, Handle handle)
	{
		throw null;
	}

	public static GuidHandle GuidHandle(int offset)
	{
		throw null;
	}

	public static Handle Handle(int token)
	{
		throw null;
	}

	public static EntityHandle Handle(TableIndex tableIndex, int rowNumber)
	{
		throw null;
	}

	public static ImportScopeHandle ImportScopeHandle(int rowNumber)
	{
		throw null;
	}

	public static InterfaceImplementationHandle InterfaceImplementationHandle(int rowNumber)
	{
		throw null;
	}

	public static LocalConstantHandle LocalConstantHandle(int rowNumber)
	{
		throw null;
	}

	public static LocalScopeHandle LocalScopeHandle(int rowNumber)
	{
		throw null;
	}

	public static LocalVariableHandle LocalVariableHandle(int rowNumber)
	{
		throw null;
	}

	public static ManifestResourceHandle ManifestResourceHandle(int rowNumber)
	{
		throw null;
	}

	public static MemberReferenceHandle MemberReferenceHandle(int rowNumber)
	{
		throw null;
	}

	public static MethodDebugInformationHandle MethodDebugInformationHandle(int rowNumber)
	{
		throw null;
	}

	public static MethodDefinitionHandle MethodDefinitionHandle(int rowNumber)
	{
		throw null;
	}

	public static MethodImplementationHandle MethodImplementationHandle(int rowNumber)
	{
		throw null;
	}

	public static MethodSpecificationHandle MethodSpecificationHandle(int rowNumber)
	{
		throw null;
	}

	public static ModuleReferenceHandle ModuleReferenceHandle(int rowNumber)
	{
		throw null;
	}

	public static ParameterHandle ParameterHandle(int rowNumber)
	{
		throw null;
	}

	public static PropertyDefinitionHandle PropertyDefinitionHandle(int rowNumber)
	{
		throw null;
	}

	public static StandaloneSignatureHandle StandaloneSignatureHandle(int rowNumber)
	{
		throw null;
	}

	public static StringHandle StringHandle(int offset)
	{
		throw null;
	}

	public static bool TryGetHeapIndex(HandleKind type, out HeapIndex index)
	{
		throw null;
	}

	public static bool TryGetTableIndex(HandleKind type, out TableIndex index)
	{
		throw null;
	}

	public static TypeDefinitionHandle TypeDefinitionHandle(int rowNumber)
	{
		throw null;
	}

	public static TypeReferenceHandle TypeReferenceHandle(int rowNumber)
	{
		throw null;
	}

	public static TypeSpecificationHandle TypeSpecificationHandle(int rowNumber)
	{
		throw null;
	}

	public static UserStringHandle UserStringHandle(int offset)
	{
		throw null;
	}
}
