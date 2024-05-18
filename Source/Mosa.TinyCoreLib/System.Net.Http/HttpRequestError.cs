namespace System.Net.Http;

public enum HttpRequestError
{
	Unknown,
	NameResolutionError,
	ConnectionError,
	SecureConnectionError,
	HttpProtocolError,
	ExtendedConnectNotSupported,
	VersionNegotiationError,
	UserAuthenticationError,
	ProxyTunnelError,
	InvalidResponse,
	ResponseEnded,
	ConfigurationLimitExceeded
}
