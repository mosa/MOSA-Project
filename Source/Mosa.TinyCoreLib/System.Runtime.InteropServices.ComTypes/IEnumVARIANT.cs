using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumVARIANT
{
	IEnumVARIANT Clone();

	int Next(int celt, object?[] rgVar, IntPtr pceltFetched);

	int Reset();

	int Skip(int celt);
}
