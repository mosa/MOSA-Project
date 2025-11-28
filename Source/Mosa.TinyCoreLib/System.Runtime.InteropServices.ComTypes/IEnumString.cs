using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumString
{
	void Clone(out IEnumString ppenum);

	int Next(int celt, string[] rgelt, IntPtr pceltFetched);

	void Reset();

	int Skip(int celt);
}
