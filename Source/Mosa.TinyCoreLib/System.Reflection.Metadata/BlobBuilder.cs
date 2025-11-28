using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace System.Reflection.Metadata;

public class BlobBuilder
{
	public struct Blobs : IEnumerable<Blob>, IEnumerable, IEnumerator<Blob>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public Blob Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public Blobs GetEnumerator()
		{
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
		}

		IEnumerator<Blob> IEnumerable<Blob>.GetEnumerator()
		{
			throw null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}

		void IDisposable.Dispose()
		{
		}
	}

	protected internal int ChunkCapacity
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	protected int FreeBytes
	{
		get
		{
			throw null;
		}
	}

	public BlobBuilder(int capacity = 256)
	{
	}

	public void Align(int alignment)
	{
	}

	protected virtual BlobBuilder AllocateChunk(int minimalSize)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool ContentEquals(BlobBuilder other)
	{
		throw null;
	}

	protected void Free()
	{
	}

	protected virtual void FreeChunk()
	{
	}

	public Blobs GetBlobs()
	{
		throw null;
	}

	public void LinkPrefix(BlobBuilder prefix)
	{
	}

	public void LinkSuffix(BlobBuilder suffix)
	{
	}

	public void PadTo(int position)
	{
	}

	public Blob ReserveBytes(int byteCount)
	{
		throw null;
	}

	public byte[] ToArray()
	{
		throw null;
	}

	public byte[] ToArray(int start, int byteCount)
	{
		throw null;
	}

	public ImmutableArray<byte> ToImmutableArray()
	{
		throw null;
	}

	public ImmutableArray<byte> ToImmutableArray(int start, int byteCount)
	{
		throw null;
	}

	public int TryWriteBytes(Stream source, int byteCount)
	{
		throw null;
	}

	public void WriteBoolean(bool value)
	{
	}

	public void WriteByte(byte value)
	{
	}

	public unsafe void WriteBytes(byte* buffer, int byteCount)
	{
	}

	public void WriteBytes(byte value, int byteCount)
	{
	}

	public void WriteBytes(byte[] buffer)
	{
	}

	public void WriteBytes(byte[] buffer, int start, int byteCount)
	{
	}

	public void WriteBytes(ImmutableArray<byte> buffer)
	{
	}

	public void WriteBytes(ImmutableArray<byte> buffer, int start, int byteCount)
	{
	}

	public void WriteCompressedInteger(int value)
	{
	}

	public void WriteCompressedSignedInteger(int value)
	{
	}

	public void WriteConstant(object? value)
	{
	}

	public void WriteContentTo(Stream destination)
	{
	}

	public void WriteContentTo(BlobBuilder destination)
	{
	}

	public void WriteContentTo(ref BlobWriter destination)
	{
	}

	public void WriteDateTime(DateTime value)
	{
	}

	public void WriteDecimal(decimal value)
	{
	}

	public void WriteDouble(double value)
	{
	}

	public void WriteGuid(Guid value)
	{
	}

	public void WriteInt16(short value)
	{
	}

	public void WriteInt16BE(short value)
	{
	}

	public void WriteInt32(int value)
	{
	}

	public void WriteInt32BE(int value)
	{
	}

	public void WriteInt64(long value)
	{
	}

	public void WriteReference(int reference, bool isSmall)
	{
	}

	public void WriteSByte(sbyte value)
	{
	}

	public void WriteSerializedString(string? value)
	{
	}

	public void WriteSingle(float value)
	{
	}

	public void WriteUInt16(ushort value)
	{
	}

	public void WriteUInt16BE(ushort value)
	{
	}

	public void WriteUInt32(uint value)
	{
	}

	public void WriteUInt32BE(uint value)
	{
	}

	public void WriteUInt64(ulong value)
	{
	}

	public void WriteUserString(string value)
	{
	}

	public void WriteUTF16(char[] value)
	{
	}

	public void WriteUTF16(string value)
	{
	}

	public void WriteUTF8(string value, bool allowUnpairedSurrogates = true)
	{
	}
}
