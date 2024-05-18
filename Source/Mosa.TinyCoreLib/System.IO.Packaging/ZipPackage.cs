namespace System.IO.Packaging;

public sealed class ZipPackage : Package
{
	internal ZipPackage()
		: base((FileAccess)0)
	{
	}

	protected override PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption)
	{
		throw null;
	}

	protected override void DeletePartCore(Uri partUri)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override void FlushCore()
	{
	}

	protected override PackagePart? GetPartCore(Uri partUri)
	{
		throw null;
	}

	protected override PackagePart[] GetPartsCore()
	{
		throw null;
	}
}
