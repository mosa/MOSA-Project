namespace System;

public static class AppContext
{
	public static string BaseDirectory
	{
		get
		{
			throw null;
		}
	}

	public static string? TargetFrameworkName
	{
		get
		{
			throw null;
		}
	}

	public static object? GetData(string name)
	{
		throw null;
	}

	public static void SetData(string name, object? data)
	{
	}

	public static void SetSwitch(string switchName, bool isEnabled)
	{
	}

	public static bool TryGetSwitch(string switchName, out bool isEnabled)
	{
		throw null;
	}
}
