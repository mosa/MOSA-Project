using System.Text;

namespace System.Net.Http;

public delegate Encoding? HeaderEncodingSelector<TContext>(string headerName, TContext context);
