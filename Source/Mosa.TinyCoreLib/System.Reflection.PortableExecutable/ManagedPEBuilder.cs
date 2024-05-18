using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.PortableExecutable;

public class ManagedPEBuilder : PEBuilder
{
	public const int ManagedResourcesDataAlignment = 8;

	public const int MappedFieldDataAlignment = 8;

	public ManagedPEBuilder(PEHeaderBuilder header, MetadataRootBuilder metadataRootBuilder, BlobBuilder ilStream, BlobBuilder? mappedFieldData = null, BlobBuilder? managedResources = null, ResourceSectionBuilder? nativeResources = null, DebugDirectoryBuilder? debugDirectoryBuilder = null, int strongNameSignatureSize = 128, MethodDefinitionHandle entryPoint = default(MethodDefinitionHandle), CorFlags flags = CorFlags.ILOnly, Func<IEnumerable<Blob>, BlobContentId>? deterministicIdProvider = null)
		: base(null, null)
	{
	}

	protected override ImmutableArray<Section> CreateSections()
	{
		throw null;
	}

	protected internal override PEDirectoriesBuilder GetDirectories()
	{
		throw null;
	}

	protected override BlobBuilder SerializeSection(string name, SectionLocation location)
	{
		throw null;
	}

	public void Sign(BlobBuilder peImage, Func<IEnumerable<Blob>, byte[]> signatureProvider)
	{
	}
}
