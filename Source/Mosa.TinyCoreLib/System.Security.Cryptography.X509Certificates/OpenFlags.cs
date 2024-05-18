namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum OpenFlags
{
	ReadOnly = 0,
	ReadWrite = 1,
	MaxAllowed = 2,
	OpenExistingOnly = 4,
	IncludeArchived = 8
}
