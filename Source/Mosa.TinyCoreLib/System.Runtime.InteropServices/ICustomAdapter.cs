using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface ICustomAdapter
{
	object GetUnderlyingObject();
}
