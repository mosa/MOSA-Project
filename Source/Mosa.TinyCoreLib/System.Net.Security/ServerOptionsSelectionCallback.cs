using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Security;

public delegate ValueTask<SslServerAuthenticationOptions> ServerOptionsSelectionCallback(SslStream stream, SslClientHelloInfo clientHelloInfo, object? state, CancellationToken cancellationToken);
