using System.Reflection.Metadata;

namespace System.Reflection.PortableExecutable;

public abstract class ResourceSectionBuilder
{
	protected internal abstract void Serialize(BlobBuilder builder, SectionLocation location);
}
