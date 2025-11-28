using System.Runtime.InteropServices;

namespace System.Diagnostics;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICollectData
{
	void CloseData();

	void CollectData(int id, IntPtr valueName, IntPtr data, int totalBytes, out IntPtr res);
}
