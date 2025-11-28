using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace System.Net;

public class WebClient : Component
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public bool AllowReadStreamBuffering
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public bool AllowWriteStreamBuffering
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string BaseAddress
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

	public RequestCachePolicy? CachePolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICredentials? Credentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WebHeaderCollection Headers
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

	public bool IsBusy
	{
		get
		{
			throw null;
		}
	}

	public IWebProxy? Proxy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public NameValueCollection QueryString
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

	public WebHeaderCollection? ResponseHeaders
	{
		get
		{
			throw null;
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event DownloadDataCompletedEventHandler? DownloadDataCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event AsyncCompletedEventHandler? DownloadFileCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DownloadProgressChangedEventHandler? DownloadProgressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event DownloadStringCompletedEventHandler? DownloadStringCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event OpenReadCompletedEventHandler? OpenReadCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event OpenWriteCompletedEventHandler? OpenWriteCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UploadDataCompletedEventHandler? UploadDataCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UploadFileCompletedEventHandler? UploadFileCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UploadProgressChangedEventHandler? UploadProgressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UploadStringCompletedEventHandler? UploadStringCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UploadValuesCompletedEventHandler? UploadValuesCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public event WriteStreamClosedEventHandler? WriteStreamClosed
	{
		add
		{
		}
		remove
		{
		}
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public WebClient()
	{
	}

	public void CancelAsync()
	{
	}

	public byte[] DownloadData(string address)
	{
		throw null;
	}

	public byte[] DownloadData(Uri address)
	{
		throw null;
	}

	public void DownloadDataAsync(Uri address)
	{
	}

	public void DownloadDataAsync(Uri address, object? userToken)
	{
	}

	public Task<byte[]> DownloadDataTaskAsync(string address)
	{
		throw null;
	}

	public Task<byte[]> DownloadDataTaskAsync(Uri address)
	{
		throw null;
	}

	public void DownloadFile(string address, string fileName)
	{
	}

	public void DownloadFile(Uri address, string fileName)
	{
	}

	public void DownloadFileAsync(Uri address, string fileName)
	{
	}

	public void DownloadFileAsync(Uri address, string fileName, object? userToken)
	{
	}

	public Task DownloadFileTaskAsync(string address, string fileName)
	{
		throw null;
	}

	public Task DownloadFileTaskAsync(Uri address, string fileName)
	{
		throw null;
	}

	public string DownloadString(string address)
	{
		throw null;
	}

	public string DownloadString(Uri address)
	{
		throw null;
	}

	public void DownloadStringAsync(Uri address)
	{
	}

	public void DownloadStringAsync(Uri address, object? userToken)
	{
	}

	public Task<string> DownloadStringTaskAsync(string address)
	{
		throw null;
	}

	public Task<string> DownloadStringTaskAsync(Uri address)
	{
		throw null;
	}

	protected virtual WebRequest GetWebRequest(Uri address)
	{
		throw null;
	}

	protected virtual WebResponse GetWebResponse(WebRequest request)
	{
		throw null;
	}

	protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
	{
		throw null;
	}

	protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
	{
	}

	protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
	{
	}

	protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
	{
	}

	protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
	{
	}

	protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs e)
	{
	}

	protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs e)
	{
	}

	protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs e)
	{
	}

	protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs e)
	{
	}

	protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
	{
	}

	protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
	{
	}

	protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	protected virtual void OnWriteStreamClosed(WriteStreamClosedEventArgs e)
	{
	}

	public Stream OpenRead(string address)
	{
		throw null;
	}

	public Stream OpenRead(Uri address)
	{
		throw null;
	}

	public void OpenReadAsync(Uri address)
	{
	}

	public void OpenReadAsync(Uri address, object? userToken)
	{
	}

	public Task<Stream> OpenReadTaskAsync(string address)
	{
		throw null;
	}

	public Task<Stream> OpenReadTaskAsync(Uri address)
	{
		throw null;
	}

	public Stream OpenWrite(string address)
	{
		throw null;
	}

	public Stream OpenWrite(string address, string? method)
	{
		throw null;
	}

	public Stream OpenWrite(Uri address)
	{
		throw null;
	}

	public Stream OpenWrite(Uri address, string? method)
	{
		throw null;
	}

	public void OpenWriteAsync(Uri address)
	{
	}

	public void OpenWriteAsync(Uri address, string? method)
	{
	}

	public void OpenWriteAsync(Uri address, string? method, object? userToken)
	{
	}

	public Task<Stream> OpenWriteTaskAsync(string address)
	{
		throw null;
	}

	public Task<Stream> OpenWriteTaskAsync(string address, string? method)
	{
		throw null;
	}

	public Task<Stream> OpenWriteTaskAsync(Uri address)
	{
		throw null;
	}

	public Task<Stream> OpenWriteTaskAsync(Uri address, string? method)
	{
		throw null;
	}

	public byte[] UploadData(string address, byte[] data)
	{
		throw null;
	}

	public byte[] UploadData(string address, string? method, byte[] data)
	{
		throw null;
	}

	public byte[] UploadData(Uri address, byte[] data)
	{
		throw null;
	}

	public byte[] UploadData(Uri address, string? method, byte[] data)
	{
		throw null;
	}

	public void UploadDataAsync(Uri address, byte[] data)
	{
	}

	public void UploadDataAsync(Uri address, string? method, byte[] data)
	{
	}

	public void UploadDataAsync(Uri address, string? method, byte[] data, object? userToken)
	{
	}

	public Task<byte[]> UploadDataTaskAsync(string address, byte[] data)
	{
		throw null;
	}

	public Task<byte[]> UploadDataTaskAsync(string address, string? method, byte[] data)
	{
		throw null;
	}

	public Task<byte[]> UploadDataTaskAsync(Uri address, byte[] data)
	{
		throw null;
	}

	public Task<byte[]> UploadDataTaskAsync(Uri address, string? method, byte[] data)
	{
		throw null;
	}

	public byte[] UploadFile(string address, string fileName)
	{
		throw null;
	}

	public byte[] UploadFile(string address, string? method, string fileName)
	{
		throw null;
	}

	public byte[] UploadFile(Uri address, string fileName)
	{
		throw null;
	}

	public byte[] UploadFile(Uri address, string? method, string fileName)
	{
		throw null;
	}

	public void UploadFileAsync(Uri address, string fileName)
	{
	}

	public void UploadFileAsync(Uri address, string? method, string fileName)
	{
	}

	public void UploadFileAsync(Uri address, string? method, string fileName, object? userToken)
	{
	}

	public Task<byte[]> UploadFileTaskAsync(string address, string fileName)
	{
		throw null;
	}

	public Task<byte[]> UploadFileTaskAsync(string address, string? method, string fileName)
	{
		throw null;
	}

	public Task<byte[]> UploadFileTaskAsync(Uri address, string fileName)
	{
		throw null;
	}

	public Task<byte[]> UploadFileTaskAsync(Uri address, string? method, string fileName)
	{
		throw null;
	}

	public string UploadString(string address, string data)
	{
		throw null;
	}

	public string UploadString(string address, string? method, string data)
	{
		throw null;
	}

	public string UploadString(Uri address, string data)
	{
		throw null;
	}

	public string UploadString(Uri address, string? method, string data)
	{
		throw null;
	}

	public void UploadStringAsync(Uri address, string data)
	{
	}

	public void UploadStringAsync(Uri address, string? method, string data)
	{
	}

	public void UploadStringAsync(Uri address, string? method, string data, object? userToken)
	{
	}

	public Task<string> UploadStringTaskAsync(string address, string data)
	{
		throw null;
	}

	public Task<string> UploadStringTaskAsync(string address, string? method, string data)
	{
		throw null;
	}

	public Task<string> UploadStringTaskAsync(Uri address, string data)
	{
		throw null;
	}

	public Task<string> UploadStringTaskAsync(Uri address, string? method, string data)
	{
		throw null;
	}

	public byte[] UploadValues(string address, NameValueCollection data)
	{
		throw null;
	}

	public byte[] UploadValues(string address, string? method, NameValueCollection data)
	{
		throw null;
	}

	public byte[] UploadValues(Uri address, NameValueCollection data)
	{
		throw null;
	}

	public byte[] UploadValues(Uri address, string? method, NameValueCollection data)
	{
		throw null;
	}

	public void UploadValuesAsync(Uri address, NameValueCollection data)
	{
	}

	public void UploadValuesAsync(Uri address, string? method, NameValueCollection data)
	{
	}

	public void UploadValuesAsync(Uri address, string? method, NameValueCollection data, object? userToken)
	{
	}

	public Task<byte[]> UploadValuesTaskAsync(string address, NameValueCollection data)
	{
		throw null;
	}

	public Task<byte[]> UploadValuesTaskAsync(string address, string? method, NameValueCollection data)
	{
		throw null;
	}

	public Task<byte[]> UploadValuesTaskAsync(Uri address, NameValueCollection data)
	{
		throw null;
	}

	public Task<byte[]> UploadValuesTaskAsync(Uri address, string? method, NameValueCollection data)
	{
		throw null;
	}
}
