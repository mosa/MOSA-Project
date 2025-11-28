namespace System.IO.Packaging;

public sealed class ZipPackagePart : PackagePart
{
	internal ZipPackagePart()
		: base(null, null)
	{
	}

	protected override Stream? GetStreamCore(FileMode streamFileMode, FileAccess streamFileAccess)
	{
		throw null;
	}
}
