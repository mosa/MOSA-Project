namespace System.Configuration;

public enum ConfigurationAllowExeDefinition
{
	MachineOnly = 0,
	MachineToApplication = 100,
	MachineToRoamingUser = 200,
	MachineToLocalUser = 300
}
