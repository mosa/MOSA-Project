namespace System.ComponentModel;

public abstract class License : IDisposable
{
	public abstract string LicenseKey { get; }

	public abstract void Dispose();
}
