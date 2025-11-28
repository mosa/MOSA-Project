namespace System.IO.Packaging;

public abstract class PackagePart
{
	public CompressionOption CompressionOption
	{
		get
		{
			throw null;
		}
	}

	public string ContentType
	{
		get
		{
			throw null;
		}
	}

	public Package Package
	{
		get
		{
			throw null;
		}
	}

	public Uri Uri
	{
		get
		{
			throw null;
		}
	}

	protected PackagePart(Package package, Uri partUri)
	{
	}

	protected PackagePart(Package package, Uri partUri, string? contentType)
	{
	}

	protected PackagePart(Package package, Uri partUri, string? contentType, CompressionOption compressionOption)
	{
	}

	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType)
	{
		throw null;
	}

	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string? id)
	{
		throw null;
	}

	public void DeleteRelationship(string id)
	{
	}

	protected virtual string GetContentTypeCore()
	{
		throw null;
	}

	public PackageRelationship GetRelationship(string id)
	{
		throw null;
	}

	public PackageRelationshipCollection GetRelationships()
	{
		throw null;
	}

	public PackageRelationshipCollection GetRelationshipsByType(string relationshipType)
	{
		throw null;
	}

	public Stream GetStream()
	{
		throw null;
	}

	public Stream GetStream(FileMode mode)
	{
		throw null;
	}

	public Stream GetStream(FileMode mode, FileAccess access)
	{
		throw null;
	}

	protected abstract Stream? GetStreamCore(FileMode mode, FileAccess access);

	public bool RelationshipExists(string id)
	{
		throw null;
	}
}
