namespace System.Security.AccessControl;

[Flags]
public enum PropagationFlags
{
	None = 0,
	NoPropagateInherit = 1,
	InheritOnly = 2
}
