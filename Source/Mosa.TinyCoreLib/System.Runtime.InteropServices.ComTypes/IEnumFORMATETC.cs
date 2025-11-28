using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumFORMATETC
{
	void Clone(out IEnumFORMATETC newEnum);

	int Next(int celt, FORMATETC[] rgelt, int[] pceltFetched);

	int Reset();

	int Skip(int celt);
}
