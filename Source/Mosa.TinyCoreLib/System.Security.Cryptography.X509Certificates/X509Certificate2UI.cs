namespace System.Security.Cryptography.X509Certificates;

public sealed class X509Certificate2UI
{
	public static void DisplayCertificate(X509Certificate2 certificate)
	{
	}

	public static void DisplayCertificate(X509Certificate2 certificate, IntPtr hwndParent)
	{
	}

	public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string? title, string? message, X509SelectionFlag selectionFlag)
	{
		throw null;
	}

	public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string? title, string? message, X509SelectionFlag selectionFlag, IntPtr hwndParent)
	{
		throw null;
	}
}
