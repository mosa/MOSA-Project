namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IIUnknownStrategy
{
	unsafe void* CreateInstancePointer(void* unknown);

	unsafe int QueryInterface(void* instancePtr, in Guid iid, out void* ppObj);

	unsafe int Release(void* instancePtr);
}
