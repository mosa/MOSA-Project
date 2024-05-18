using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IRunningObjectTable
{
	void EnumRunning(out IEnumMoniker ppenumMoniker);

	int GetObject(IMoniker pmkObjectName, out object ppunkObject);

	int GetTimeOfLastChange(IMoniker pmkObjectName, out FILETIME pfiletime);

	int IsRunning(IMoniker pmkObjectName);

	void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

	int Register(int grfFlags, object punkObject, IMoniker pmkObjectName);

	void Revoke(int dwRegister);
}
