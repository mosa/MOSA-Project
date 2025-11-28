using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Security;
using System.Text;

namespace System.Diagnostics;

public sealed class ProcessStartInfo
{
	public Collection<string> ArgumentList
	{
		get
		{
			throw null;
		}
	}

	public string Arguments
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public bool CreateNoWindow
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public string Domain
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public IDictionary<string, string?> Environment
	{
		get
		{
			throw null;
		}
	}

	[Editor("System.Diagnostics.Design.StringDictionaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public StringDictionary EnvironmentVariables
	{
		get
		{
			throw null;
		}
	}

	public bool ErrorDialog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr ErrorDialogParentHandle
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string FileName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public bool LoadUserProfile
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public bool UseCredentialsForNetworkingOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[CLSCompliant(false)]
	[SupportedOSPlatform("windows")]
	public SecureString? Password
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public string? PasswordInClearText
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RedirectStandardError
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RedirectStandardInput
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool RedirectStandardOutput
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding? StandardErrorEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding? StandardInputEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding? StandardOutputEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string UserName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public bool UseShellExecute
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue("")]
	public string Verb
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string[] Verbs
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(ProcessWindowStyle.Normal)]
	public ProcessWindowStyle WindowStyle
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string WorkingDirectory
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public ProcessStartInfo()
	{
	}

	public ProcessStartInfo(string fileName)
	{
	}

	public ProcessStartInfo(string fileName, string arguments)
	{
	}

	public ProcessStartInfo(string fileName, IEnumerable<string> arguments)
	{
	}
}
