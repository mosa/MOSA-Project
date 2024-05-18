using System.IO;
using System.Security;

namespace System.Configuration.Internal;

public class DelegatingConfigHost : IInternalConfigHost
{
	public virtual bool HasLocalConfig
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasRoamingConfig
	{
		get
		{
			throw null;
		}
	}

	protected IInternalConfigHost Host
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IsAppConfigHttp
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsRemote
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsChangeNotifications
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsLocation
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsPath
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsRefresh
	{
		get
		{
			throw null;
		}
	}

	protected DelegatingConfigHost()
	{
	}

	public virtual object CreateConfigurationContext(string configPath, string locationSubPath)
	{
		throw null;
	}

	public virtual object CreateDeprecatedConfigContext(string configPath)
	{
		throw null;
	}

	public virtual string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
	{
		throw null;
	}

	public virtual void DeleteStream(string streamName)
	{
	}

	public virtual string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
	{
		throw null;
	}

	public virtual string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath)
	{
		throw null;
	}

	public virtual Type GetConfigType(string typeName, bool throwOnError)
	{
		throw null;
	}

	public virtual string GetConfigTypeName(Type t)
	{
		throw null;
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
	{
		throw null;
	}

	public virtual string GetStreamName(string configPath)
	{
		throw null;
	}

	public virtual string GetStreamNameForConfigSource(string streamName, string configSource)
	{
		throw null;
	}

	public virtual object GetStreamVersion(string streamName)
	{
		throw null;
	}

	public virtual IDisposable Impersonate()
	{
		throw null;
	}

	public virtual void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
	{
	}

	public virtual void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
	{
		throw null;
	}

	public virtual bool IsAboveApplication(string configPath)
	{
		throw null;
	}

	public virtual bool IsConfigRecordRequired(string configPath)
	{
		throw null;
	}

	public virtual bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
	{
		throw null;
	}

	public virtual bool IsFile(string streamName)
	{
		throw null;
	}

	public virtual bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord)
	{
		throw null;
	}

	public virtual bool IsInitDelayed(IInternalConfigRecord configRecord)
	{
		throw null;
	}

	public virtual bool IsLocationApplicable(string configPath)
	{
		throw null;
	}

	public virtual bool IsSecondaryRoot(string configPath)
	{
		throw null;
	}

	public virtual bool IsTrustedConfigPath(string configPath)
	{
		throw null;
	}

	public virtual Stream OpenStreamForRead(string streamName)
	{
		throw null;
	}

	public virtual Stream OpenStreamForRead(string streamName, bool assertPermissions)
	{
		throw null;
	}

	public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
	{
		throw null;
	}

	public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
	{
		throw null;
	}

	public virtual bool PrefetchAll(string configPath, string streamName)
	{
		throw null;
	}

	public virtual bool PrefetchSection(string sectionGroupName, string sectionName)
	{
		throw null;
	}

	public virtual void RefreshConfigPaths()
	{
	}

	public virtual void RequireCompleteInit(IInternalConfigRecord configRecord)
	{
	}

	public virtual object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
	{
		throw null;
	}

	public virtual void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
	{
	}

	public virtual void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo)
	{
	}

	public virtual void WriteCompleted(string streamName, bool success, object writeContext)
	{
	}

	public virtual void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions)
	{
	}
}
