using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum CustomQueryInterfaceResult
{
	Handled,
	NotHandled,
	Failed
}
