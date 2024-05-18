using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMoniker
{
	void BindToObject(IBindCtx pbc, IMoniker? pmkToLeft, ref Guid riidResult, out object ppvResult);

	void BindToStorage(IBindCtx pbc, IMoniker? pmkToLeft, ref Guid riid, out object ppvObj);

	void CommonPrefixWith(IMoniker pmkOther, out IMoniker? ppmkPrefix);

	void ComposeWith(IMoniker pmkRight, bool fOnlyIfNotGeneric, out IMoniker? ppmkComposite);

	void Enum(bool fForward, out IEnumMoniker? ppenumMoniker);

	void GetClassID(out Guid pClassID);

	void GetDisplayName(IBindCtx pbc, IMoniker? pmkToLeft, out string ppszDisplayName);

	void GetSizeMax(out long pcbSize);

	void GetTimeOfLastChange(IBindCtx pbc, IMoniker? pmkToLeft, out FILETIME pFileTime);

	void Hash(out int pdwHash);

	void Inverse(out IMoniker ppmk);

	int IsDirty();

	int IsEqual(IMoniker pmkOtherMoniker);

	int IsRunning(IBindCtx pbc, IMoniker? pmkToLeft, IMoniker? pmkNewlyRunning);

	int IsSystemMoniker(out int pdwMksys);

	void Load(IStream pStm);

	void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);

	void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker? ppmkToLeft, out IMoniker? ppmkReduced);

	void RelativePathTo(IMoniker pmkOther, out IMoniker? ppmkRelPath);

	void Save(IStream pStm, bool fClearDirty);
}
