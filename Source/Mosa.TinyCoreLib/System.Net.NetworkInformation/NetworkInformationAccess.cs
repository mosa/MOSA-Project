namespace System.Net.NetworkInformation;

[Flags]
public enum NetworkInformationAccess
{
	None = 0,
	Read = 1,
	Ping = 4
}
