using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum DESCKIND
{
	DESCKIND_NONE,
	DESCKIND_FUNCDESC,
	DESCKIND_VARDESC,
	DESCKIND_TYPECOMP,
	DESCKIND_IMPLICITAPPOBJ,
	DESCKIND_MAX
}
