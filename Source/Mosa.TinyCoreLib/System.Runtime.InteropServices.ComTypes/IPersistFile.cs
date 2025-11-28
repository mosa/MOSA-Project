using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPersistFile
{
	void GetClassID(out Guid pClassID);

	void GetCurFile(out string ppszFileName);

	int IsDirty();

	void Load(string pszFileName, int dwMode);

	void Save(string? pszFileName, bool fRemember);

	void SaveCompleted(string pszFileName);
}
