using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface ICustomQueryInterface
{
	CustomQueryInterfaceResult GetInterface(ref Guid iid, out IntPtr ppv);
}
