// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;

namespace Mosa.Demo.Application;

/// <summary>
/// BaseApplication
/// </summary>
public abstract class BaseApplication
{
	public AppConsole Console { get; private set; }

	public AppManager AppManager { get; set; }

	protected BaseApplication()
	{
		Console = new AppConsole();
	}

	/// <summary>
	/// Starts this application
	/// </summary>
	/// <returns>
	/// error code
	/// </returns>
	public abstract int Start(string parameters);

	public static unsafe void DumpStackTrace(int line)
	{
		AppManager.DumpStackTrace(line);
	}

	public static unsafe void DumpData(string data)
	{
		AppManager.DumpData(data);
	}
}
