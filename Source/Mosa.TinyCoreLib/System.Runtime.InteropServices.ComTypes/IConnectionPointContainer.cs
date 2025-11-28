using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IConnectionPointContainer
{
	void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

	void FindConnectionPoint(ref Guid riid, out IConnectionPoint? ppCP);
}
