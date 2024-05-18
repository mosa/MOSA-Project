namespace System.Net;

public enum WebExceptionStatus
{
	Success,
	NameResolutionFailure,
	ConnectFailure,
	ReceiveFailure,
	SendFailure,
	PipelineFailure,
	RequestCanceled,
	ProtocolError,
	ConnectionClosed,
	TrustFailure,
	SecureChannelFailure,
	ServerProtocolViolation,
	KeepAliveFailure,
	Pending,
	Timeout,
	ProxyNameResolutionFailure,
	UnknownError,
	MessageLengthLimitExceeded,
	CacheEntryNotFound,
	RequestProhibitedByCachePolicy,
	RequestProhibitedByProxy
}
