using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public sealed class MetadataBuilder
{
	public MetadataBuilder(int userStringHeapStartOffset = 0, int stringHeapStartOffset = 0, int blobHeapStartOffset = 0, int guidHeapStartOffset = 0)
	{
	}

	public AssemblyDefinitionHandle AddAssembly(StringHandle name, Version version, StringHandle culture, BlobHandle publicKey, AssemblyFlags flags, AssemblyHashAlgorithm hashAlgorithm)
	{
		throw null;
	}

	public AssemblyFileHandle AddAssemblyFile(StringHandle name, BlobHandle hashValue, bool containsMetadata)
	{
		throw null;
	}

	public AssemblyReferenceHandle AddAssemblyReference(StringHandle name, Version version, StringHandle culture, BlobHandle publicKeyOrToken, AssemblyFlags flags, BlobHandle hashValue)
	{
		throw null;
	}

	public ConstantHandle AddConstant(EntityHandle parent, object? value)
	{
		throw null;
	}

	public CustomAttributeHandle AddCustomAttribute(EntityHandle parent, EntityHandle constructor, BlobHandle value)
	{
		throw null;
	}

	public CustomDebugInformationHandle AddCustomDebugInformation(EntityHandle parent, GuidHandle kind, BlobHandle value)
	{
		throw null;
	}

	public DeclarativeSecurityAttributeHandle AddDeclarativeSecurityAttribute(EntityHandle parent, DeclarativeSecurityAction action, BlobHandle permissionSet)
	{
		throw null;
	}

	public DocumentHandle AddDocument(BlobHandle name, GuidHandle hashAlgorithm, BlobHandle hash, GuidHandle language)
	{
		throw null;
	}

	public void AddEncLogEntry(EntityHandle entity, EditAndContinueOperation code)
	{
	}

	public void AddEncMapEntry(EntityHandle entity)
	{
	}

	public EventDefinitionHandle AddEvent(EventAttributes attributes, StringHandle name, EntityHandle type)
	{
		throw null;
	}

	public void AddEventMap(TypeDefinitionHandle declaringType, EventDefinitionHandle eventList)
	{
	}

	public ExportedTypeHandle AddExportedType(TypeAttributes attributes, StringHandle @namespace, StringHandle name, EntityHandle implementation, int typeDefinitionId)
	{
		throw null;
	}

	public FieldDefinitionHandle AddFieldDefinition(FieldAttributes attributes, StringHandle name, BlobHandle signature)
	{
		throw null;
	}

	public void AddFieldLayout(FieldDefinitionHandle field, int offset)
	{
	}

	public void AddFieldRelativeVirtualAddress(FieldDefinitionHandle field, int offset)
	{
	}

	public GenericParameterHandle AddGenericParameter(EntityHandle parent, GenericParameterAttributes attributes, StringHandle name, int index)
	{
		throw null;
	}

	public GenericParameterConstraintHandle AddGenericParameterConstraint(GenericParameterHandle genericParameter, EntityHandle constraint)
	{
		throw null;
	}

	public ImportScopeHandle AddImportScope(ImportScopeHandle parentScope, BlobHandle imports)
	{
		throw null;
	}

	public InterfaceImplementationHandle AddInterfaceImplementation(TypeDefinitionHandle type, EntityHandle implementedInterface)
	{
		throw null;
	}

	public LocalConstantHandle AddLocalConstant(StringHandle name, BlobHandle signature)
	{
		throw null;
	}

	public LocalScopeHandle AddLocalScope(MethodDefinitionHandle method, ImportScopeHandle importScope, LocalVariableHandle variableList, LocalConstantHandle constantList, int startOffset, int length)
	{
		throw null;
	}

	public LocalVariableHandle AddLocalVariable(LocalVariableAttributes attributes, int index, StringHandle name)
	{
		throw null;
	}

	public ManifestResourceHandle AddManifestResource(ManifestResourceAttributes attributes, StringHandle name, EntityHandle implementation, uint offset)
	{
		throw null;
	}

	public void AddMarshallingDescriptor(EntityHandle parent, BlobHandle descriptor)
	{
	}

	public MemberReferenceHandle AddMemberReference(EntityHandle parent, StringHandle name, BlobHandle signature)
	{
		throw null;
	}

	public MethodDebugInformationHandle AddMethodDebugInformation(DocumentHandle document, BlobHandle sequencePoints)
	{
		throw null;
	}

	public MethodDefinitionHandle AddMethodDefinition(MethodAttributes attributes, MethodImplAttributes implAttributes, StringHandle name, BlobHandle signature, int bodyOffset, ParameterHandle parameterList)
	{
		throw null;
	}

	public MethodImplementationHandle AddMethodImplementation(TypeDefinitionHandle type, EntityHandle methodBody, EntityHandle methodDeclaration)
	{
		throw null;
	}

	public void AddMethodImport(MethodDefinitionHandle method, MethodImportAttributes attributes, StringHandle name, ModuleReferenceHandle module)
	{
	}

	public void AddMethodSemantics(EntityHandle association, MethodSemanticsAttributes semantics, MethodDefinitionHandle methodDefinition)
	{
	}

	public MethodSpecificationHandle AddMethodSpecification(EntityHandle method, BlobHandle instantiation)
	{
		throw null;
	}

	public ModuleDefinitionHandle AddModule(int generation, StringHandle moduleName, GuidHandle mvid, GuidHandle encId, GuidHandle encBaseId)
	{
		throw null;
	}

	public ModuleReferenceHandle AddModuleReference(StringHandle moduleName)
	{
		throw null;
	}

	public void AddNestedType(TypeDefinitionHandle type, TypeDefinitionHandle enclosingType)
	{
	}

	public ParameterHandle AddParameter(ParameterAttributes attributes, StringHandle name, int sequenceNumber)
	{
		throw null;
	}

	public PropertyDefinitionHandle AddProperty(PropertyAttributes attributes, StringHandle name, BlobHandle signature)
	{
		throw null;
	}

	public void AddPropertyMap(TypeDefinitionHandle declaringType, PropertyDefinitionHandle propertyList)
	{
	}

	public StandaloneSignatureHandle AddStandaloneSignature(BlobHandle signature)
	{
		throw null;
	}

	public void AddStateMachineMethod(MethodDefinitionHandle moveNextMethod, MethodDefinitionHandle kickoffMethod)
	{
	}

	public TypeDefinitionHandle AddTypeDefinition(TypeAttributes attributes, StringHandle @namespace, StringHandle name, EntityHandle baseType, FieldDefinitionHandle fieldList, MethodDefinitionHandle methodList)
	{
		throw null;
	}

	public void AddTypeLayout(TypeDefinitionHandle type, ushort packingSize, uint size)
	{
	}

	public TypeReferenceHandle AddTypeReference(EntityHandle resolutionScope, StringHandle @namespace, StringHandle name)
	{
		throw null;
	}

	public TypeSpecificationHandle AddTypeSpecification(BlobHandle signature)
	{
		throw null;
	}

	public BlobHandle GetOrAddBlob(byte[] value)
	{
		throw null;
	}

	public BlobHandle GetOrAddBlob(ImmutableArray<byte> value)
	{
		throw null;
	}

	public BlobHandle GetOrAddBlob(BlobBuilder value)
	{
		throw null;
	}

	public BlobHandle GetOrAddBlobUTF16(string value)
	{
		throw null;
	}

	public BlobHandle GetOrAddBlobUTF8(string value, bool allowUnpairedSurrogates = true)
	{
		throw null;
	}

	public BlobHandle GetOrAddConstantBlob(object? value)
	{
		throw null;
	}

	public BlobHandle GetOrAddDocumentName(string value)
	{
		throw null;
	}

	public GuidHandle GetOrAddGuid(Guid guid)
	{
		throw null;
	}

	public StringHandle GetOrAddString(string value)
	{
		throw null;
	}

	public UserStringHandle GetOrAddUserString(string value)
	{
		throw null;
	}

	public int GetRowCount(TableIndex table)
	{
		throw null;
	}

	public ImmutableArray<int> GetRowCounts()
	{
		throw null;
	}

	public ReservedBlob<GuidHandle> ReserveGuid()
	{
		throw null;
	}

	public ReservedBlob<UserStringHandle> ReserveUserString(int length)
	{
		throw null;
	}

	public void SetCapacity(HeapIndex heap, int byteCount)
	{
	}

	public void SetCapacity(TableIndex table, int rowCount)
	{
	}
}
