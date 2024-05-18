using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal;

public class WindowsIdentity : ClaimsIdentity, IDisposable, IDeserializationCallback, ISerializable
{
	public new const string DefaultIssuer = "AD AUTHORITY";

	public SafeAccessTokenHandle AccessToken
	{
		get
		{
			throw null;
		}
	}

	public sealed override string? AuthenticationType
	{
		get
		{
			throw null;
		}
	}

	public override IEnumerable<Claim> Claims
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Claim> DeviceClaims
	{
		get
		{
			throw null;
		}
	}

	public IdentityReferenceCollection? Groups
	{
		get
		{
			throw null;
		}
	}

	public TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsAnonymous
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGuest
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSystem
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public SecurityIdentifier? Owner
	{
		get
		{
			throw null;
		}
	}

	public virtual IntPtr Token
	{
		get
		{
			throw null;
		}
	}

	public SecurityIdentifier? User
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Claim> UserClaims
	{
		get
		{
			throw null;
		}
	}

	public WindowsIdentity(IntPtr userToken)
	{
	}

	public WindowsIdentity(IntPtr userToken, string type)
	{
	}

	public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType)
	{
	}

	public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public WindowsIdentity(SerializationInfo info, StreamingContext context)
	{
	}

	protected WindowsIdentity(WindowsIdentity identity)
	{
	}

	public WindowsIdentity(string sUserPrincipalName)
	{
	}

	public override ClaimsIdentity Clone()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static WindowsIdentity GetAnonymous()
	{
		throw null;
	}

	public static WindowsIdentity GetCurrent()
	{
		throw null;
	}

	public static WindowsIdentity? GetCurrent(bool ifImpersonating)
	{
		throw null;
	}

	public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess)
	{
		throw null;
	}

	public static void RunImpersonated(SafeAccessTokenHandle safeAccessTokenHandle, Action action)
	{
	}

	public static Task RunImpersonatedAsync(SafeAccessTokenHandle safeAccessTokenHandle, Func<Task> func)
	{
		throw null;
	}

	public static Task<T> RunImpersonatedAsync<T>(SafeAccessTokenHandle safeAccessTokenHandle, Func<Task<T>> func)
	{
		throw null;
	}

	public static T RunImpersonated<T>(SafeAccessTokenHandle safeAccessTokenHandle, Func<T> func)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
