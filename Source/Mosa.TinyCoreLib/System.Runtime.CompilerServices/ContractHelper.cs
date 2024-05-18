using System.Diagnostics.Contracts;

namespace System.Runtime.CompilerServices;

public static class ContractHelper
{
	public static string? RaiseContractFailedEvent(ContractFailureKind failureKind, string? userMessage, string? conditionText, Exception? innerException)
	{
		throw null;
	}

	public static void TriggerFailure(ContractFailureKind kind, string? displayMessage, string? userMessage, string? conditionText, Exception? innerException)
	{
	}
}
