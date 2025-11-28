using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITypeInfo2 : ITypeInfo
{
	new void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

	new void CreateInstance(object? pUnkOuter, ref Guid riid, out object ppvObj);

	void GetAllCustData(IntPtr pCustData);

	void GetAllFuncCustData(int index, IntPtr pCustData);

	void GetAllImplTypeCustData(int index, IntPtr pCustData);

	void GetAllParamCustData(int indexFunc, int indexParam, IntPtr pCustData);

	void GetAllVarCustData(int index, IntPtr pCustData);

	new void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

	void GetCustData(ref Guid guid, out object pVarVal);

	new void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

	new void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

	void GetDocumentation2(int memid, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

	void GetFuncCustData(int index, ref Guid guid, out object pVarVal);

	new void GetFuncDesc(int index, out IntPtr ppFuncDesc);

	void GetFuncIndexOfMemId(int memid, INVOKEKIND invKind, out int pFuncIndex);

	new void GetIDsOfNames(string[] rgszNames, int cNames, int[] pMemId);

	void GetImplTypeCustData(int index, ref Guid guid, out object pVarVal);

	new void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

	new void GetMops(int memid, out string? pBstrMops);

	new void GetNames(int memid, string[] rgBstrNames, int cMaxNames, out int pcNames);

	void GetParamCustData(int indexFunc, int indexParam, ref Guid guid, out object pVarVal);

	new void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

	new void GetRefTypeOfImplType(int index, out int href);

	new void GetTypeAttr(out IntPtr ppTypeAttr);

	new void GetTypeComp(out ITypeComp ppTComp);

	void GetTypeFlags(out int pTypeFlags);

	void GetTypeKind(out TYPEKIND pTypeKind);

	void GetVarCustData(int index, ref Guid guid, out object pVarVal);

	new void GetVarDesc(int index, out IntPtr ppVarDesc);

	void GetVarIndexOfMemId(int memid, out int pVarIndex);

	new void Invoke(object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

	new void ReleaseFuncDesc(IntPtr pFuncDesc);

	new void ReleaseTypeAttr(IntPtr pTypeAttr);

	new void ReleaseVarDesc(IntPtr pVarDesc);
}
