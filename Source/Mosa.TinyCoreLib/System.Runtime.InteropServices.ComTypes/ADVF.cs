using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum ADVF
{
	ADVF_NODATA = 1,
	ADVF_PRIMEFIRST = 2,
	ADVF_ONLYONCE = 4,
	ADVFCACHE_NOHANDLER = 8,
	ADVFCACHE_FORCEBUILTIN = 0x10,
	ADVFCACHE_ONSAVE = 0x20,
	ADVF_DATAONSTOP = 0x40
}
