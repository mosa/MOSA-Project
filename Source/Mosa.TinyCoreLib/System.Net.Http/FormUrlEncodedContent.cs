using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class FormUrlEncodedContent : ByteArrayContent
{
	public FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
		: base(null)
	{
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}
}
