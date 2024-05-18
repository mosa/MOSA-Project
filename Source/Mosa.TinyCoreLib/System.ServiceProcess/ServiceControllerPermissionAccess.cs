namespace System.ServiceProcess;

[Flags]
public enum ServiceControllerPermissionAccess
{
	None = 0,
	Browse = 2,
	Control = 6
}
