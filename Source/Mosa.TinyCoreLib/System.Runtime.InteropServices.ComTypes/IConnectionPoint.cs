using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IConnectionPoint
{
	void Advise(object pUnkSink, out int pdwCookie);

	void EnumConnections(out IEnumConnections ppEnum);

	void GetConnectionInterface(out Guid pIID);

	void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

	void Unadvise(int dwCookie);
}
