using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAdviseSink
{
	void OnClose();

	void OnDataChange(ref FORMATETC format, ref STGMEDIUM stgmedium);

	void OnRename(IMoniker moniker);

	void OnSave();

	void OnViewChange(int aspect, int index);
}
