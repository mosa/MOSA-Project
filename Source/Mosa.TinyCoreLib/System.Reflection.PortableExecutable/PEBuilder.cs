using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable;

public abstract class PEBuilder
{
	protected readonly struct Section
	{
		public readonly SectionCharacteristics Characteristics;

		public readonly string Name;

		public Section(string name, SectionCharacteristics characteristics)
		{
			throw null;
		}
	}

	public PEHeaderBuilder Header
	{
		get
		{
			throw null;
		}
	}

	public Func<IEnumerable<Blob>, BlobContentId> IdProvider
	{
		get
		{
			throw null;
		}
	}

	public bool IsDeterministic
	{
		get
		{
			throw null;
		}
	}

	protected PEBuilder(PEHeaderBuilder header, Func<IEnumerable<Blob>, BlobContentId>? deterministicIdProvider)
	{
	}

	protected abstract ImmutableArray<Section> CreateSections();

	protected internal abstract PEDirectoriesBuilder GetDirectories();

	protected ImmutableArray<Section> GetSections()
	{
		throw null;
	}

	public BlobContentId Serialize(BlobBuilder builder)
	{
		throw null;
	}

	protected abstract BlobBuilder SerializeSection(string name, SectionLocation location);
}
