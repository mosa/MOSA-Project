namespace System.Reflection;

public enum DeclarativeSecurityAction : short
{
	None = 0,
	Demand = 2,
	Assert = 3,
	Deny = 4,
	PermitOnly = 5,
	LinkDemand = 6,
	InheritanceDemand = 7,
	RequestMinimum = 8,
	RequestOptional = 9,
	RequestRefuse = 10
}
