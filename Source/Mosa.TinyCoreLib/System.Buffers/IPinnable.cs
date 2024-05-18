namespace System.Buffers;

public interface IPinnable
{
	MemoryHandle Pin(int elementIndex);

	void Unpin();
}
