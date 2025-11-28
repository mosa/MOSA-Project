namespace System.Configuration.Internal;

public interface IConfigErrorInfo
{
	string Filename { get; }

	int LineNumber { get; }
}
