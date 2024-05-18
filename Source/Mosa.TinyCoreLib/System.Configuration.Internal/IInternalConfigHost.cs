using System.IO;
using System.Security;

namespace System.Configuration.Internal;

public interface IInternalConfigHost
{
	bool IsRemote { get; }

	bool SupportsChangeNotifications { get; }

	bool SupportsLocation { get; }

	bool SupportsPath { get; }

	bool SupportsRefresh { get; }

	object CreateConfigurationContext(string configPath, string locationSubPath);

	object CreateDeprecatedConfigContext(string configPath);

	string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);

	void DeleteStream(string streamName);

	string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);

	string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath);

	Type GetConfigType(string typeName, bool throwOnError);

	string GetConfigTypeName(Type t);

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady);

	string GetStreamName(string configPath);

	string GetStreamNameForConfigSource(string streamName, string configSource);

	object GetStreamVersion(string streamName);

	IDisposable Impersonate();

	void Init(IInternalConfigRoot configRoot, params object[] hostInitParams);

	void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams);

	bool IsAboveApplication(string configPath);

	bool IsConfigRecordRequired(string configPath);

	bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition);

	bool IsFile(string streamName);

	bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord);

	bool IsInitDelayed(IInternalConfigRecord configRecord);

	bool IsLocationApplicable(string configPath);

	bool IsSecondaryRoot(string configPath);

	bool IsTrustedConfigPath(string configPath);

	Stream OpenStreamForRead(string streamName);

	Stream OpenStreamForRead(string streamName, bool assertPermissions);

	Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext);

	Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions);

	bool PrefetchAll(string configPath, string streamName);

	bool PrefetchSection(string sectionGroupName, string sectionName);

	void RequireCompleteInit(IInternalConfigRecord configRecord);

	object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);

	void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);

	void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo);

	void WriteCompleted(string streamName, bool success, object writeContext);

	void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions);
}
