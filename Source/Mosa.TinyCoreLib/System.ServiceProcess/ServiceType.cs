namespace System.ServiceProcess;

[Flags]
public enum ServiceType
{
	KernelDriver = 1,
	FileSystemDriver = 2,
	Adapter = 4,
	RecognizerDriver = 8,
	Win32OwnProcess = 0x10,
	Win32ShareProcess = 0x20,
	InteractiveProcess = 0x100
}
