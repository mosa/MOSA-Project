using System.ComponentModel;

namespace System;

public enum PlatformID
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	Win32S,
	[EditorBrowsable(EditorBrowsableState.Never)]
	Win32Windows,
	Win32NT,
	[EditorBrowsable(EditorBrowsableState.Never)]
	WinCE,
	Unix,
	[EditorBrowsable(EditorBrowsableState.Never)]
	Xbox,
	[EditorBrowsable(EditorBrowsableState.Never)]
	MacOSX,
	Other
}
