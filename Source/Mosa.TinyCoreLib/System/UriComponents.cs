namespace System;

[Flags]
public enum UriComponents
{
	SerializationInfoString = int.MinValue,
	Scheme = 1,
	UserInfo = 2,
	Host = 4,
	Port = 8,
	SchemeAndServer = 0xD,
	Path = 0x10,
	Query = 0x20,
	PathAndQuery = 0x30,
	HttpRequestUrl = 0x3D,
	Fragment = 0x40,
	AbsoluteUri = 0x7F,
	StrongPort = 0x80,
	HostAndPort = 0x84,
	StrongAuthority = 0x86,
	NormalizedHost = 0x100,
	KeepDelimiter = 0x40000000
}
