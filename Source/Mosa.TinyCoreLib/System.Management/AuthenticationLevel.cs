namespace System.Management;

public enum AuthenticationLevel
{
	Unchanged = -1,
	Default,
	None,
	Connect,
	Call,
	Packet,
	PacketIntegrity,
	PacketPrivacy
}
