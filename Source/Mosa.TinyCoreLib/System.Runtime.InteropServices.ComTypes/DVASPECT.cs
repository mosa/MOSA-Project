using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum DVASPECT
{
	DVASPECT_CONTENT = 1,
	DVASPECT_THUMBNAIL = 2,
	DVASPECT_ICON = 4,
	DVASPECT_DOCPRINT = 8
}
