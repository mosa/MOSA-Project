using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class MemoryStream : Stream
{
	public override bool CanRead => !closed;

	public override bool CanSeek => !closed;

	public override bool CanWrite => canWrite;

	public virtual int Capacity
	{
		get
		{
			if (closed)
				Internal.Exceptions.Stream.ThrowClosedStreamException();

			return backingCapacity;
		}
		set
		{
			if (closed)
				Internal.Exceptions.Stream.ThrowClosedStreamException();

			if (!resizable)
				Internal.Exceptions.Generic.NotSupported();

			ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
			ArgumentOutOfRangeException.ThrowIfLessThan(value, length, nameof(value));

			backingCapacity = value;

			var newBuffer = new byte[value];

			Array.Copy(backingBuffer, newBuffer, length);
			backingBuffer = newBuffer;
		}
	}

	public override long Length
	{
		get
		{
			if (closed)
				Internal.Exceptions.Stream.ThrowClosedStreamException();

			return length;
		}
	}

	public override long Position
	{
		get
		{
			if (closed)
				Internal.Exceptions.Stream.ThrowClosedStreamException();

			return position;
		}
		set
		{
			if (closed)
				Internal.Exceptions.Stream.ThrowClosedStreamException();

			ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
			ArgumentOutOfRangeException.ThrowIfGreaterThan(value, int.MaxValue, nameof(value));

			position = value;
		}
	}

	private int backingCapacity;
	private long length, position;
	private bool canWrite, resizable, isPubliclyVisible;
	private byte[] backingBuffer;

	public MemoryStream()
	{
		canWrite = true;
		resizable = true;
		isPubliclyVisible = true;
		backingBuffer = [];
	}

	public MemoryStream(byte[] buffer)
	{
		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

		backingCapacity = buffer.Length;
		length = backingCapacity;
		canWrite = true;
		backingBuffer = buffer;
	}

	public MemoryStream(byte[] buffer, bool writable)
	{
		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

		backingCapacity = buffer.Length;
		length = backingCapacity;
		canWrite = writable;
		backingBuffer = buffer;
	}

	public MemoryStream(byte[] buffer, int index, int count)
	{
		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (buffer.Length - index < count)
			Internal.Exceptions.MemoryStream.ThrowBufferTooSmallException(nameof(buffer));

		backingCapacity = count;
		length = backingCapacity;
		canWrite = true;
		backingBuffer = buffer[index..count];
	}

	public MemoryStream(byte[] buffer, int index, int count, bool writable)
	{
		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (buffer.Length - index < count)
			Internal.Exceptions.MemoryStream.ThrowBufferTooSmallException(nameof(buffer));

		backingCapacity = count;
		length = backingCapacity;
		canWrite = writable;
		backingBuffer = buffer[index..count];
	}

	public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
	{
		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
		ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (buffer.Length - index < count)
			Internal.Exceptions.MemoryStream.ThrowBufferTooSmallException(nameof(buffer));

		backingCapacity = count;
		length = backingCapacity;
		canWrite = writable;
		isPubliclyVisible = publiclyVisible;
		backingBuffer = buffer[index..count];
	}

	public MemoryStream(int capacity)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));

		backingCapacity = capacity;
		canWrite = true;
		resizable = true;
		isPubliclyVisible = true;
		backingBuffer = new byte[capacity];
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		=> throw new NotImplementedException();

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		=> throw new NotImplementedException();

	public override void CopyTo(Stream destination, int bufferSize)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		ArgumentNullException.ThrowIfNull(destination, nameof(destination));
		ArgumentOutOfRangeException.ThrowIfNegative(bufferSize, nameof(bufferSize));

		if (!destination.CanWrite)
			Internal.Exceptions.Generic.NotSupported();

		var buffer = new byte[bufferSize];
		int bytes;

		do
		{
			bytes = Read(buffer);
			destination.Write(buffer);
		} while (bytes == bufferSize);
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		=> throw new NotImplementedException();

	protected override void Dispose(bool disposing) { }

	public override int EndRead(IAsyncResult asyncResult)
		=> throw new NotImplementedException();

	public override void EndWrite(IAsyncResult asyncResult)
		=> throw new NotImplementedException();

	public override void Flush() { }

	public override Task FlushAsync(CancellationToken cancellationToken)
		=> throw new NotImplementedException();

	// TODO: Do we need to get the entire byte array from the constructor if one was
	// specified, and not just the sliced array?
	public virtual byte[] GetBuffer()
	{
		if (isPubliclyVisible)
			return backingBuffer;

		Internal.Exceptions.MemoryStream.ThrowBufferNotVisibleException();
		return null;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
		ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (buffer.Length - offset < count)
			Internal.Exceptions.MemoryStream.ThrowBufferTooSmallException(nameof(buffer));

		var bytes = 0;

		for (var i = offset; i < count; i++)
		{
			if (position >= length)
				break;

			buffer[i] = backingBuffer[position++];
			bytes++;
		}

		return bytes;
	}

	public override int Read(Span<byte> buffer)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		var bytes = 0;

		for (var i = 0; i < buffer.Length; i++)
		{
			if (position >= length)
				break;

			buffer[i] = backingBuffer[position++];
			bytes++;
		}

		return bytes;
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		=> throw new NotImplementedException();

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		=> throw new NotImplementedException();

	public override int ReadByte()
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		if (position >= length)
			return -1;

		return backingBuffer[position++];
	}

	// TODO: ArgumentException when arithmetic overflow?
	public override long Seek(long offset, SeekOrigin loc)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		ArgumentOutOfRangeException.ThrowIfGreaterThan(offset, int.MaxValue, nameof(offset));

		long newPosition;

		switch (loc)
		{
			case SeekOrigin.Begin:
				{
					newPosition = offset;
					break;
				}
			case SeekOrigin.Current:
				{
					newPosition = position + offset;
					break;
				}
			case SeekOrigin.End:
				{
					newPosition = length + offset;
					break;
				}
			default:
				{
					Internal.Exceptions.MemoryStream.ThrowInvalidSeekOriginException(nameof(loc));
					return 0;
				}
		}

		if (newPosition < 0)
			Internal.Exceptions.MemoryStream.ThrowSeekingBeforeBeginningException();

		position = newPosition;
		return newPosition;
	}

	public override void SetLength(long value)
	{
		if (!resizable && value > backingCapacity || !canWrite)
			Internal.Exceptions.Generic.NotSupported();

		ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value, int.MaxValue, nameof(value));

		if (resizable && value > backingCapacity)
			Capacity = (int)value; // We set the property so it can resize the backing buffer

		if (value > length)
			for (var i = length; i < value; i++)
				backingBuffer[i] = 0;

		length = value;
	}

	public virtual byte[] ToArray()
	{
		var buffer = new byte[length];

		Array.Copy(backingBuffer, buffer, length);
		return buffer;
	}

	public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
	{
		if (!isPubliclyVisible)
		{
			buffer = default;
			return false;
		}

		buffer = GetBuffer();
		return true;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		if (!canWrite)
			Internal.Exceptions.Generic.NotSupported();

		ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
		ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
		ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

		if (buffer.Length - offset < count)
			Internal.Exceptions.MemoryStream.ThrowBufferTooSmallException(nameof(buffer));

		if (position + count >= length)
		{
			// Get the excess byte count and multiply it by a constant
			Capacity += (int)((position + count - length + 1) * Internal.Impl.MemoryStream.NextCapacityMultiplySize);
		}

		for (var i = offset; i < count; i++)
		{
			backingBuffer[position++] = buffer[i];
			length++;
		}
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		if (!canWrite)
			Internal.Exceptions.Generic.NotSupported();

		if (position + buffer.Length >= length)
		{
			// Get the excess byte count and multiply it by a constant
			Capacity += (int)((position + buffer.Length - length + 1) * Internal.Impl.MemoryStream.NextCapacityMultiplySize);
		}

		foreach (var b in buffer)
		{
			backingBuffer[position++] = b;
			length++;
		}
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		=> throw new NotImplementedException();

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		=> throw new NotImplementedException();

	public override void WriteByte(byte value)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		if (!canWrite)
			Internal.Exceptions.Generic.NotSupported();

		if (position >= length)
		{
			// Get the excess byte count and multiply it by a constant
			Capacity += (int)((position - length + 1) * Internal.Impl.MemoryStream.NextCapacityMultiplySize);
		}

		backingBuffer[position++] = value;
		length++;
	}

	public virtual void WriteTo(Stream stream)
	{
		if (closed)
			Internal.Exceptions.Stream.ThrowClosedStreamException();

		ArgumentNullException.ThrowIfNull(stream);

		stream.Write(backingBuffer, 0, (int)length);
	}
}
