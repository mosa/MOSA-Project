using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

public enum PosixSignal
{
	[UnsupportedOSPlatform("windows")]
	SIGTSTP = -10,
	[UnsupportedOSPlatform("windows")]
	SIGTTOU,
	[UnsupportedOSPlatform("windows")]
	SIGTTIN,
	[UnsupportedOSPlatform("windows")]
	SIGWINCH,
	[UnsupportedOSPlatform("windows")]
	SIGCONT,
	[UnsupportedOSPlatform("windows")]
	SIGCHLD,
	SIGTERM,
	SIGQUIT,
	SIGINT,
	SIGHUP
}
