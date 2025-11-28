using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IBindCtx
{
	void EnumObjectParam(out IEnumString? ppenum);

	void GetBindOptions(ref BIND_OPTS pbindopts);

	void GetObjectParam(string pszKey, out object? ppunk);

	void GetRunningObjectTable(out IRunningObjectTable? pprot);

	void RegisterObjectBound(object punk);

	void RegisterObjectParam(string pszKey, object punk);

	void ReleaseBoundObjects();

	void RevokeObjectBound(object punk);

	int RevokeObjectParam(string pszKey);

	void SetBindOptions(ref BIND_OPTS pbindopts);
}
