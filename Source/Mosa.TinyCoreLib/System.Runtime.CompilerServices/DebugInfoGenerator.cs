using System.Linq.Expressions;

namespace System.Runtime.CompilerServices;

public abstract class DebugInfoGenerator
{
	[Obsolete("The CreatePdbGenerator API is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0008", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static DebugInfoGenerator CreatePdbGenerator()
	{
		throw null;
	}

	public abstract void MarkSequencePoint(LambdaExpression method, int ilOffset, DebugInfoExpression sequencePoint);
}
