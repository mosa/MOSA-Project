using System.ComponentModel;

namespace System.Security.Cryptography;

public enum CipherMode
{
	CBC = 1,
	ECB,
	[EditorBrowsable(EditorBrowsableState.Never)]
	OFB,
	CFB,
	CTS
}
