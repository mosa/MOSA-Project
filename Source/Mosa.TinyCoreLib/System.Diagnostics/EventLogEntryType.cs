namespace System.Diagnostics;

public enum EventLogEntryType
{
	Error = 1,
	Warning = 2,
	Information = 4,
	SuccessAudit = 8,
	FailureAudit = 0x10
}
