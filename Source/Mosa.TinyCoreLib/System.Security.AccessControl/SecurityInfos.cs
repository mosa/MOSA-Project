namespace System.Security.AccessControl;

[Flags]
public enum SecurityInfos
{
	Owner = 1,
	Group = 2,
	DiscretionaryAcl = 4,
	SystemAcl = 8
}
