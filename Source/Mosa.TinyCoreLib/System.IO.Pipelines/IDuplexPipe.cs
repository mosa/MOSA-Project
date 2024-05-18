namespace System.IO.Pipelines;

public interface IDuplexPipe
{
	PipeReader Input { get; }

	PipeWriter Output { get; }
}
