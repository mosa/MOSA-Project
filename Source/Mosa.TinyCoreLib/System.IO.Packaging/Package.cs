namespace System.IO.Packaging;

public abstract class Package : IDisposable
{
	public FileAccess FileOpenAccess
	{
		get
		{
			throw null;
		}
	}

	public PackageProperties PackageProperties
	{
		get
		{
			throw null;
		}
	}

	protected Package(FileAccess openFileAccess)
	{
	}

	public void Close()
	{
	}

	public PackagePart CreatePart(Uri partUri, string contentType)
	{
		throw null;
	}

	public PackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption)
	{
		throw null;
	}

	protected abstract PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption);

	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType)
	{
		throw null;
	}

	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string? id)
	{
		throw null;
	}

	public void DeletePart(Uri partUri)
	{
	}

	protected abstract void DeletePartCore(Uri partUri);

	public void DeleteRelationship(string id)
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Flush()
	{
	}

	protected abstract void FlushCore();

	public PackagePart GetPart(Uri partUri)
	{
		throw null;
	}

	protected abstract PackagePart? GetPartCore(Uri partUri);

	public PackagePartCollection GetParts()
	{
		throw null;
	}

	protected abstract PackagePart[] GetPartsCore();

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

	public static Package Open(Stream stream)
	{
		throw null;
	}

	public static Package Open(Stream stream, FileMode packageMode)
	{
		throw null;
	}

	public static Package Open(Stream stream, FileMode packageMode, FileAccess packageAccess)
	{
		throw null;
	}

	public static Package Open(string path)
	{
		throw null;
	}

	public static Package Open(string path, FileMode packageMode)
	{
		throw null;
	}

	public static Package Open(string path, FileMode packageMode, FileAccess packageAccess)
	{
		throw null;
	}

	public static Package Open(string path, FileMode packageMode, FileAccess packageAccess, FileShare packageShare)
	{
		throw null;
	}

	public virtual bool PartExists(Uri partUri)
	{
		throw null;
	}

	public bool RelationshipExists(string id)
	{
		throw null;
	}

	void IDisposable.Dispose()
	{
	}
}
