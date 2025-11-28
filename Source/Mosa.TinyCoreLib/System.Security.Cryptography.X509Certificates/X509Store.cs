namespace System.Security.Cryptography.X509Certificates;

public sealed class X509Store : IDisposable
{
	public X509Certificate2Collection Certificates
	{
		get
		{
			throw null;
		}
	}

	public bool IsOpen
	{
		get
		{
			throw null;
		}
	}

	public StoreLocation Location
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public IntPtr StoreHandle
	{
		get
		{
			throw null;
		}
	}

	public X509Store()
	{
	}

	public X509Store(IntPtr storeHandle)
	{
	}

	public X509Store(StoreLocation storeLocation)
	{
	}

	public X509Store(StoreName storeName)
	{
	}

	public X509Store(StoreName storeName, StoreLocation storeLocation)
	{
	}

	public X509Store(StoreName storeName, StoreLocation storeLocation, OpenFlags flags)
	{
	}

	public X509Store(string storeName)
	{
	}

	public X509Store(string storeName, StoreLocation storeLocation)
	{
	}

	public X509Store(string storeName, StoreLocation storeLocation, OpenFlags flags)
	{
	}

	public void Add(X509Certificate2 certificate)
	{
	}

	public void AddRange(X509Certificate2Collection certificates)
	{
	}

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	public void Open(OpenFlags flags)
	{
	}

	public void Remove(X509Certificate2 certificate)
	{
	}

	public void RemoveRange(X509Certificate2Collection certificates)
	{
	}
}
