namespace System.IO.Ports;

public enum SerialPinChange
{
	CtsChanged = 8,
	DsrChanged = 0x10,
	CDChanged = 0x20,
	Break = 0x40,
	Ring = 0x100
}
