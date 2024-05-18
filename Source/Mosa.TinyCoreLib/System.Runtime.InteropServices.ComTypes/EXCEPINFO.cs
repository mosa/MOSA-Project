using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct EXCEPINFO
{
	public string bstrDescription;

	public string bstrHelpFile;

	public string bstrSource;

	public int dwHelpContext;

	public IntPtr pfnDeferredFillIn;

	public IntPtr pvReserved;

	public int scode;

	public short wCode;

	public short wReserved;
}
