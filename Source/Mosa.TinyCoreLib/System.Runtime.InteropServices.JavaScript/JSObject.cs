using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[SupportedOSPlatform("browser")]
public class JSObject : IDisposable
{
	public bool IsDisposed
	{
		get
		{
			throw null;
		}
	}

	internal JSObject()
	{
		throw null;
	}

	public void Dispose()
	{
		throw null;
	}

	public bool HasProperty(string propertyName)
	{
		throw null;
	}

	public string GetTypeOfProperty(string propertyName)
	{
		throw null;
	}

	public bool GetPropertyAsBoolean(string propertyName)
	{
		throw null;
	}

	public int GetPropertyAsInt32(string propertyName)
	{
		throw null;
	}

	public double GetPropertyAsDouble(string propertyName)
	{
		throw null;
	}

	public string? GetPropertyAsString(string propertyName)
	{
		throw null;
	}

	public JSObject? GetPropertyAsJSObject(string propertyName)
	{
		throw null;
	}

	public byte[]? GetPropertyAsByteArray(string propertyName)
	{
		throw null;
	}

	public void SetProperty(string propertyName, bool value)
	{
		throw null;
	}

	public void SetProperty(string propertyName, int value)
	{
		throw null;
	}

	public void SetProperty(string propertyName, double value)
	{
		throw null;
	}

	public void SetProperty(string propertyName, string? value)
	{
		throw null;
	}

	public void SetProperty(string propertyName, JSObject? value)
	{
		throw null;
	}

	public void SetProperty(string propertyName, byte[]? value)
	{
		throw null;
	}
}
