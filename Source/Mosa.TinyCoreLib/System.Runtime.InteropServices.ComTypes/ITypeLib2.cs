using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITypeLib2 : ITypeLib
{
	new void FindName(string szNameBuf, int lHashVal, ITypeInfo[] ppTInfo, int[] rgMemId, ref short pcFound);

	void GetAllCustData(IntPtr pCustData);

	void GetCustData(ref Guid guid, out object pVarVal);

	new void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

	void GetDocumentation2(int index, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

	new void GetLibAttr(out IntPtr ppTLibAttr);

	void GetLibStatistics(IntPtr pcUniqueNames, out int pcchUniqueNames);

	new void GetTypeComp(out ITypeComp ppTComp);

	new void GetTypeInfo(int index, out ITypeInfo ppTI);

	new int GetTypeInfoCount();

	new void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

	new void GetTypeInfoType(int index, out TYPEKIND pTKind);

	new bool IsName(string szNameBuf, int lHashVal);

	new void ReleaseTLibAttr(IntPtr pTLibAttr);
}
