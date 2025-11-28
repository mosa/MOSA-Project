namespace System.Net.Security;

public enum NegotiateAuthenticationStatusCode
{
	Completed,
	ContinueNeeded,
	GenericFailure,
	BadBinding,
	Unsupported,
	MessageAltered,
	ContextExpired,
	CredentialsExpired,
	InvalidCredentials,
	InvalidToken,
	UnknownCredentials,
	QopNotSupported,
	OutOfSequence,
	SecurityQosFailed,
	TargetUnknown,
	ImpersonationValidationFailed
}
