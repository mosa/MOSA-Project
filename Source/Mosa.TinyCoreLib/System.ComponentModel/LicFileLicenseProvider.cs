namespace System.ComponentModel;

public class LicFileLicenseProvider : LicenseProvider
{
	protected virtual string GetKey(Type type)
	{
		throw null;
	}

	public override License? GetLicense(LicenseContext context, Type type, object? instance, bool allowExceptions)
	{
		throw null;
	}

	protected virtual bool IsKeyValid(string? key, Type type)
	{
		throw null;
	}
}
