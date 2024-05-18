using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IStream
{
	void Clone(out IStream ppstm);

	void Commit(int grfCommitFlags);

	void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

	void LockRegion(long libOffset, long cb, int dwLockType);

	void Read(byte[] pv, int cb, IntPtr pcbRead);

	void Revert();

	void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

	void SetSize(long libNewSize);

	void Stat(out STATSTG pstatstg, int grfStatFlag);

	void UnlockRegion(long libOffset, long cb, int dwLockType);

	void Write(byte[] pv, int cb, IntPtr pcbWritten);
}
