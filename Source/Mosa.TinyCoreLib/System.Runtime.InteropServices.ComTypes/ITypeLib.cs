using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITypeLib
{
	void FindName(string szNameBuf, int lHashVal, ITypeInfo[] ppTInfo, int[] rgMemId, ref short pcFound);

	void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

	void GetLibAttr(out IntPtr ppTLibAttr);

	void GetTypeComp(out ITypeComp ppTComp);

	void GetTypeInfo(int index, out ITypeInfo ppTI);

	int GetTypeInfoCount();

	void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

	void GetTypeInfoType(int index, out TYPEKIND pTKind);

	bool IsName(string szNameBuf, int lHashVal);

	void ReleaseTLibAttr(IntPtr pTLibAttr);
}
