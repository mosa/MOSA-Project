namespace System.Diagnostics.Contracts;

public enum ContractFailureKind
{
	Precondition,
	Postcondition,
	PostconditionOnException,
	Invariant,
	Assert,
	Assume
}
