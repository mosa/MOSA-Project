using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum FUNCKIND
{
	FUNC_VIRTUAL,
	FUNC_PUREVIRTUAL,
	FUNC_NONVIRTUAL,
	FUNC_STATIC,
	FUNC_DISPATCH
}
