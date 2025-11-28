using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumConnectionPoints
{
	void Clone(out IEnumConnectionPoints ppenum);

	int Next(int celt, IConnectionPoint[] rgelt, IntPtr pceltFetched);

	void Reset();

	int Skip(int celt);
}
