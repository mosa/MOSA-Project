using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumMoniker
{
	void Clone(out IEnumMoniker ppenum);

	int Next(int celt, IMoniker[] rgelt, IntPtr pceltFetched);

	void Reset();

	int Skip(int celt);
}
