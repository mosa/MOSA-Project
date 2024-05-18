namespace System.ComponentModel;

public abstract class LicenseProvider
{
	public abstract License? GetLicense(LicenseContext context, Type type, object? instance, bool allowExceptions);
}
