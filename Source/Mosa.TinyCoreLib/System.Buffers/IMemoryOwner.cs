namespace System.Buffers;

public interface IMemoryOwner<T> : IDisposable
{
	Memory<T> Memory { get; }
}
