using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITypeInfo
{
	void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

	void CreateInstance(object? pUnkOuter, ref Guid riid, out object ppvObj);

	void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

	void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

	void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

	void GetFuncDesc(int index, out IntPtr ppFuncDesc);

	void GetIDsOfNames(string[] rgszNames, int cNames, int[] pMemId);

	void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

	void GetMops(int memid, out string? pBstrMops);

	void GetNames(int memid, string[] rgBstrNames, int cMaxNames, out int pcNames);

	void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

	void GetRefTypeOfImplType(int index, out int href);

	void GetTypeAttr(out IntPtr ppTypeAttr);

	void GetTypeComp(out ITypeComp ppTComp);

	void GetVarDesc(int index, out IntPtr ppVarDesc);

	void Invoke(object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

	void ReleaseFuncDesc(IntPtr pFuncDesc);

	void ReleaseTypeAttr(IntPtr pTypeAttr);

	void ReleaseVarDesc(IntPtr pVarDesc);
}
