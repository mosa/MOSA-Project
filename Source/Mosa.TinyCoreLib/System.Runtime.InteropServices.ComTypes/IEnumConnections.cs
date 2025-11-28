using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumConnections
{
	void Clone(out IEnumConnections ppenum);

	int Next(int celt, CONNECTDATA[] rgelt, IntPtr pceltFetched);

	void Reset();

	int Skip(int celt);
}
