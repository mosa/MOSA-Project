using System.Collections.Generic;
using System.Globalization;
using System.Security;

namespace System.Diagnostics.Eventing.Reader;

public class EventLogSession : IDisposable
{
	public static EventLogSession GlobalSession
	{
		get
		{
			throw null;
		}
	}

	public EventLogSession()
	{
	}

	public EventLogSession(string server)
	{
	}

	public EventLogSession(string server, string domain, string user, SecureString password, SessionAuthentication logOnType)
	{
	}

	public void CancelCurrentOperations()
	{
	}

	public void ClearLog(string logName)
	{
	}

	public void ClearLog(string logName, string backupPath)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void ExportLog(string path, PathType pathType, string query, string targetFilePath)
	{
	}

	public void ExportLog(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors)
	{
	}

	public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath)
	{
	}

	public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors, CultureInfo targetCultureInfo)
	{
	}

	public EventLogInformation GetLogInformation(string logName, PathType pathType)
	{
		throw null;
	}

	public IEnumerable<string> GetLogNames()
	{
		throw null;
	}

	public IEnumerable<string> GetProviderNames()
	{
		throw null;
	}
}
