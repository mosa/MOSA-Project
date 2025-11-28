using System.Runtime.Versioning;

namespace System.Net.Sockets;

public enum IOControlCode : long
{
	[SupportedOSPlatform("windows")]
	EnableCircularQueuing = 671088642L,
	[SupportedOSPlatform("windows")]
	Flush = 671088644L,
	[SupportedOSPlatform("windows")]
	AddressListChange = 671088663L,
	DataToRead = 1074030207L,
	OobDataRead = 1074033415L,
	[SupportedOSPlatform("windows")]
	GetBroadcastAddress = 1207959557L,
	[SupportedOSPlatform("windows")]
	AddressListQuery = 1207959574L,
	[SupportedOSPlatform("windows")]
	QueryTargetPnpHandle = 1207959576L,
	[SupportedOSPlatform("windows")]
	AsyncIO = 2147772029L,
	NonBlockingIO = 2147772030L,
	[SupportedOSPlatform("windows")]
	AssociateHandle = 2281701377L,
	[SupportedOSPlatform("windows")]
	MultipointLoopback = 2281701385L,
	[SupportedOSPlatform("windows")]
	MulticastScope = 2281701386L,
	[SupportedOSPlatform("windows")]
	SetQos = 2281701387L,
	[SupportedOSPlatform("windows")]
	SetGroupQos = 2281701388L,
	[SupportedOSPlatform("windows")]
	RoutingInterfaceChange = 2281701397L,
	[SupportedOSPlatform("windows")]
	NamespaceChange = 2281701401L,
	[SupportedOSPlatform("windows")]
	ReceiveAll = 2550136833L,
	[SupportedOSPlatform("windows")]
	ReceiveAllMulticast = 2550136834L,
	[SupportedOSPlatform("windows")]
	ReceiveAllIgmpMulticast = 2550136835L,
	[SupportedOSPlatform("windows")]
	KeepAliveValues = 2550136836L,
	[SupportedOSPlatform("windows")]
	AbsorbRouterAlert = 2550136837L,
	[SupportedOSPlatform("windows")]
	UnicastInterface = 2550136838L,
	[SupportedOSPlatform("windows")]
	LimitBroadcasts = 2550136839L,
	[SupportedOSPlatform("windows")]
	BindToInterface = 2550136840L,
	[SupportedOSPlatform("windows")]
	MulticastInterface = 2550136841L,
	[SupportedOSPlatform("windows")]
	AddMulticastGroupOnInterface = 2550136842L,
	[SupportedOSPlatform("windows")]
	DeleteMulticastGroupFromInterface = 2550136843L,
	[SupportedOSPlatform("windows")]
	GetExtensionFunctionPointer = 3355443206L,
	[SupportedOSPlatform("windows")]
	GetQos = 3355443207L,
	[SupportedOSPlatform("windows")]
	GetGroupQos = 3355443208L,
	[SupportedOSPlatform("windows")]
	TranslateHandle = 3355443213L,
	[SupportedOSPlatform("windows")]
	RoutingInterfaceQuery = 3355443220L,
	[SupportedOSPlatform("windows")]
	AddressListSort = 3355443225L
}
