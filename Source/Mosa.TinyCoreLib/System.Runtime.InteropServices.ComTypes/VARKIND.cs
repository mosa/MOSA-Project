using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public enum VARKIND
{
	VAR_PERINSTANCE,
	VAR_STATIC,
	VAR_CONST,
	VAR_DISPATCH
}
